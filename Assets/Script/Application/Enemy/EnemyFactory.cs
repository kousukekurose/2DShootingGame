using UnityEngine;
using MessagePipe;
using VContainer;

namespace Project.Application.Enemy
{
    public class EnemyFactory
    {
        private readonly Project.Game.Shared.Character.EnemyConfig _defaultConfig;
        private readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _publisher;
        private readonly EnemyPrefabRegistry _prefabRegistry;
        private readonly IObjectResolver _resolver;
        private readonly EnemyManager _enemyManager;

        public EnemyFactory(
            Project.Game.Shared.Character.EnemyConfig defaultConfig,
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

        public Project.Game.Enemy.Enemy CreateEnemy(Vector3 position,Framework.Core.Interfaces.EnemyType enemyType)
        {
            var stats = _defaultConfig.GetDefaultStats();
            var attackEventPublisher = _resolver.Resolve<IPublisher<Framework.Core.Events.EnemyAttackEvent>>();
            return new Project.Game.Enemy.Enemy(
                _defaultConfig.CharacterId,
                stats,
                position,
                enemyType,
                attackEventPublisher
            );
        }

        public Presentation.Enemy.EnemyView SpawnEnemy(
            Vector3 position,Framework.Core.Interfaces.EnemyType enemyType
        )
        {
            var enemyView = _enemyManager.GetEnemyViewFromPool(enemyType);
            if(enemyView == null)
            {
                Framework.Core.CustomLogger.LogError($"Failed to spawn enemy: no prefab for {enemyType}");
                return null;
            }

            //Game層のEnemy作成
            var enemy = CreateEnemy(position,enemyType);
            enemyView.InitializeEnemy(enemy);

            //敵マネージャーに登録
            _enemyManager.RegisterEnemy(enemy);
            _enemyManager.RegisterEnemyView(enemy.CharacterId, enemyView);

            var initializer =  _resolver.Resolve<EnemyInitializer>();
            initializer.Initialize(enemy,enemyView);

            return enemyView;

        }
    }
}

