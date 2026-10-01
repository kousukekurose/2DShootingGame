using System.Collections.Generic;
using System.Linq;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.Infrastructure.Unity.Enemy
{
    public class EnemyManager
    {
        private readonly List<Project.Domain.Enemy.Enemy> _activeEnemies = new List<Project.Domain.Enemy.Enemy>();
        private readonly Dictionary<int,Project.Domain.Enemy.Enemy> _enemyRegistry = new Dictionary<int, Project.Domain.Enemy.Enemy>();
        private readonly Dictionary<string, Presentation.Enemy.EnemyView> _enemyViewMap = new Dictionary<string, Presentation.Enemy.EnemyView>();
        private readonly Dictionary<string, string> _lastStateByEnemy = new Dictionary<string, string>();
        private readonly EnemyPrefabRegistry _prefabRegistry;
        private readonly IPublisher<Project.Presentation.Events.EnemyStateChangedEvent> _stateChangedPublisher;
        private readonly Dictionary<Project.Domain.Enemy.EnemyType,ObjectPool<GameObject>> _enemyViewPools = new Dictionary<Project.Domain.Enemy.EnemyType, ObjectPool<GameObject>>();
        private Transform _enemyRootFolder;

        public EnemyManager(
            EnemyPrefabRegistry prefabRegistry,
            IPublisher<Project.Presentation.Events.EnemyStateChangedEvent> stateChangedPublisher)
        {
            _prefabRegistry = prefabRegistry;
            _stateChangedPublisher = stateChangedPublisher;
        }

        public void RegisterEnemy(Project.Domain.Enemy.Enemy enemy)
        {
            _activeEnemies.Add(enemy);
            _enemyRegistry[enemy.GetHashCode()] = enemy;
        }

        public void RegisterEnemyView(string enemyId, Presentation.Enemy.EnemyView enemyView)
        {
            _enemyViewMap[enemyId] = enemyView;
        }

        public void UnregisterEnemy(Project.Domain.Enemy.Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            _enemyRegistry.Remove(enemy.GetHashCode());
            _lastStateByEnemy.Remove(enemy.CharacterId);
        }

        public void UnregisterEnemyView(string enemyId)
        {
            _enemyViewMap.Remove(enemyId);
        }

        public IEnumerable<Project.Domain.Enemy.Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        public void UpdateAll(float deltaTime)
        {
            foreach(var enemy in _activeEnemies.ToList())
            {
                enemy.UpdateCooldownTimer(deltaTime);
                enemy.StateMachine.Update(deltaTime);
                PublishStateChange(enemy);
                if(enemy.GetCurrentPosition().y < -10f)
                {
                    if(_enemyViewMap.TryGetValue(enemy.CharacterId, out var enemyView))
                    {
                        ReturnEnemyViewToPool(enemy.EnemyType, enemyView.gameObject);
                        UnregisterEnemyView(enemy.CharacterId);
                    }
                    UnregisterEnemy(enemy);
                    continue;
                }

                if(enemy.IsDead)
                {
                    Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[EnemyManager] Enemy {enemy.CharacterId} is dead. Unregistering.");
                    // 敵Viewをプールに返却
                    if(_enemyViewMap.TryGetValue(enemy.CharacterId, out var enemyView))
                    {
                        Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[EnemyManager] Returning enemy view to pool");
                        ReturnEnemyViewToPool(enemy.EnemyType, enemyView.gameObject);
                        UnregisterEnemyView(enemy.CharacterId);
                    }
                    UnregisterEnemy(enemy);
                }
            }
        }

        private void PublishStateChange(Project.Domain.Enemy.Enemy enemy)
        {
            var currentState = enemy.StateMachine.CurrentState;
            if (currentState == null) return;

            var stateName = currentState.GetType().Name
                .Replace("Enemy", string.Empty)
                .Replace("State", string.Empty);
            if (stateName == "Move")
            {
                stateName = "Chase";
            }

            if (_lastStateByEnemy.TryGetValue(enemy.CharacterId, out var previousState)
                && previousState == stateName)
            {
                return;
            }

            _lastStateByEnemy[enemy.CharacterId] = stateName;
            _stateChangedPublisher.Publish(new Project.Presentation.Events.EnemyStateChangedEvent
            {
                EnemyId = enemy.CharacterId,
                StateName = stateName
            });
        }

        public Presentation.Enemy.EnemyView GetEnemyViewFromPool(Project.Domain.Enemy.EnemyType enemyType)
        {
            if(!_enemyViewPools.ContainsKey(enemyType))
            {
                if(_enemyRootFolder == null)
                {
                    _enemyRootFolder = new GameObject("EnemyFolder").transform;
                }
                var prefab = _prefabRegistry.GetPrefab(enemyType);
                if(prefab == null) return null;

                //プール作成
                _enemyViewPools[enemyType] = new ObjectPool<GameObject>(
                    createFunc: () => GameObject.Instantiate(prefab,_enemyRootFolder),
                    actionOnGet: view =>
                    {
                        view.SetActive(true);
                    },
                    actionOnRelease: view =>
                    {
                        view.SetActive(false);
                    },
                    actionOnDestroy: view =>
                    {
                        GameObject.Destroy(view);
                    },
                    maxSize: 50
                );
            }
            var enemyViewObject = _enemyViewPools[enemyType].Get();
            return enemyViewObject.GetComponent<Presentation.Enemy.EnemyView>();
        }

        public void ReturnEnemyViewToPool(Project.Domain.Enemy.EnemyType enemyType,GameObject enemyView)
        {
            if(_enemyViewPools.ContainsKey(enemyType))
            {
                _enemyViewPools[enemyType].Release(enemyView);
            }
        }
    }
}