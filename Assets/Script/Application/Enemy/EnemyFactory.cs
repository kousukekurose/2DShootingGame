using UnityEngine;
using MessagePipe;
using VContainer;

namespace Application.Enemy
{
    public class EnemyFactory
    {
        private readonly Game.Shared.Character.EnemyConfig _defaultConfig;
        private readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _publisher;
        private readonly EnemyPrefabRegistry _prefabRegistry;
        private readonly IObjectResolver _resolver;
        private readonly EnemyManager _enemyManager;

        public EnemyFactory(
            Game.Shared.Character.EnemyConfig defaultConfig,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> publisher,
            EnemyPrefabRegistry prefabRegistry,
            IObjectResolver resolver,
            EnemyManager enemyManager
        )
        {
            _defaultConfig = defaultConfig;
            _publisher = publisher;
            _prefabRegistry = prefabRegistry;
            _resolver = resolver;
            _enemyManager = enemyManager;
        }

        public Game.Enemy.Enemy CreateEnemy(Vector3 position,Framework.Core.Interfaces.EnemyType enemyType)
        {
            var stats = _defaultConfig.DefaultStats.Clone();
            var enemy = new Game.Enemy.Enemy(
                _defaultConfig.CharacterId,
                stats,
                position,
                enemyType
            );
            return enemy;
        }

        public Presentation.Enemy.EnemyView SpawnEnemy(
            Vector3 position,Framework.Core.Interfaces.EnemyType enemyType
        )
        {
            var prefab = _prefabRegistry.GetPrefab(enemyType);
            if(prefab == null)
            {
                Framework.Core.CustomLogger.LogError($"Failed to spawn enemy: no prefab for {enemyType}");
                return null;
            }

            var enemyObject = Object.Instantiate(prefab,position,Quaternion.identity);
            var enemyView = enemyObject.GetComponent<Presentation.Enemy.EnemyView>();

            if(enemyView == null)
            {
                Framework.Core.CustomLogger.LogError("Spawned object has no EnemyView component");
                Object.Destroy(enemyObject);
                return null;
            }

            //DIコンテナから注入
            _resolver.Inject(enemyView);
            //Game層のEnemy作成
            var enemy = CreateEnemy(position,enemyType);
            enemyView.InitializeEnemy(enemy);

            //敵マネージャーに登録
            _enemyManager.RegisterEnemy(enemy);

            Framework.Core.CustomLogger.Log($"Spawned {enemyType} enemy at {position}");
            var initializer = enemyObject.AddComponent<GameEnemyInitializer>();
            _resolver.Inject(initializer);

            return enemyView;

        }
    }
}

