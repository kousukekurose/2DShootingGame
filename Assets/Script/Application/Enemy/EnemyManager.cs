using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.AI.Navigation.LowLevel;
using UnityEngine;
using UnityEngine.Pool;

namespace Application.Enemy
{
    public class EnemyManager
    {
        private readonly List<Game.Enemy.Enemy> _activeEnemies = new List<Game.Enemy.Enemy>();
        private readonly Dictionary<int,Game.Enemy.Enemy> _enemyRegistry = new Dictionary<int, Game.Enemy.Enemy>();
        private readonly Dictionary<string, Presentation.Enemy.EnemyView> _enemyViewMap = new Dictionary<string, Presentation.Enemy.EnemyView>();
        private readonly EnemyPrefabRegistry _prefabRegistry;
        private readonly Dictionary<Framework.Core.Interfaces.EnemyType,ObjectPool<GameObject>> _enemyViewPools = new Dictionary<Framework.Core.Interfaces.EnemyType, ObjectPool<GameObject>>();
        private Transform _enemyRootFolder;

        public EnemyManager(EnemyPrefabRegistry prefabRegistry)
        {
            _prefabRegistry = prefabRegistry;
        }
        public void RegisterEnemy(Game.Enemy.Enemy enemy)
        {
            _activeEnemies.Add(enemy);
            _enemyRegistry[enemy.GetHashCode()] = enemy;
        }

        public void RegisterEnemyView(string enemyId, Presentation.Enemy.EnemyView enemyView)
        {
            _enemyViewMap[enemyId] = enemyView;
        }

        public void UnregisterEnemy(Game.Enemy.Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            _enemyRegistry.Remove(enemy.GetHashCode());
        }

        public void UnregisterEnemyView(string enemyId)
        {
            _enemyViewMap.Remove(enemyId);
        }

        public IEnumerable<Game.Enemy.Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        public void UpdateAll(float deltaTime)
        {
            foreach(var enemy in _activeEnemies.ToList())
            {
                enemy.UpdateCooldownTimer(deltaTime);
                enemy.StateMachine.Update(deltaTime);
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
                    Framework.Core.CustomLogger.Log($"[EnemyManager] Enemy {enemy.CharacterId} is dead. Unregistering.");
                    
                    // 敵Viewをプールに返却
                    if(_enemyViewMap.TryGetValue(enemy.CharacterId, out var enemyView))
                    {
                        Framework.Core.CustomLogger.Log($"[EnemyManager] Returning enemy view to pool");
                        ReturnEnemyViewToPool(enemy.EnemyType, enemyView.gameObject);
                        UnregisterEnemyView(enemy.CharacterId);
                    }
                    UnregisterEnemy(enemy);
                }
            }
        }

        public Presentation.Enemy.EnemyView GetEnemyViewFromPool(Framework.Core.Interfaces.EnemyType enemyType)
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

        public void ReturnEnemyViewToPool(Framework.Core.Interfaces.EnemyType enemyType,GameObject enemyView)
        {
            if(_enemyViewPools.ContainsKey(enemyType))
            {
                _enemyViewPools[enemyType].Release(enemyView);
            }
        }
    }
}