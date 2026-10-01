using UnityEngine;
using MessagePipe;
using VContainer;

namespace Project.Infrastructure.Unity.Enemy
{
    public class EnemyFactory
    {
        private readonly Project.Infrastructure.Unity.Config.EnemyConfig _defaultConfig;
        private readonly EnemyPrefabRegistry _prefabRegistry;
        private readonly IObjectResolver _resolver;
        private readonly EnemyManager _enemyManager;

        public EnemyFactory(
            Project.Infrastructure.Unity.Config.EnemyConfig defaultConfig,
            EnemyPrefabRegistry prefabRegistry,
            IObjectResolver resolver,
            EnemyManager enemyManager
        )
        {
            _defaultConfig = defaultConfig;
            _prefabRegistry = prefabRegistry;
            _resolver = resolver;
            _enemyManager = enemyManager;
        }

        public Project.Domain.Enemy.Enemy CreateEnemy(Vector3 position,Project.Domain.Enemy.EnemyType enemyType)
        {
            var stats = _defaultConfig.GetDefaultStats();
            var attackEventPublisher = _resolver.Resolve<IPublisher<Project.Application.Abstractions.Events.EnemyAttackEvent>>();
            return new Project.Domain.Enemy.Enemy(
                _defaultConfig.CharacterId,
                stats,
                position,
                enemyType,
                attackEventPublisher
            );
        }

        public Presentation.Enemy.EnemyView SpawnEnemy(
            Vector3 position,Project.Domain.Enemy.EnemyType enemyType
        )
        {
            var enemyView = _enemyManager.GetEnemyViewFromPool(enemyType);
            if(enemyView == null)
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.LogError($"Failed to spawn enemy: no prefab for {enemyType}");
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

