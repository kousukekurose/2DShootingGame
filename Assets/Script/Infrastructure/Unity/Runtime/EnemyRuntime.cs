using UnityEngine;
using VContainer.Unity;

namespace Project.Infrastructure.Unity.Runtime
{
    public sealed class EnemyRuntime : IStartable, ITickable
    {
        private readonly Project.Infrastructure.Unity.Enemy.EnemyPrefabRegistry _prefabRegistry;
        private readonly Project.Infrastructure.Unity.Enemy.EnemySpawner _enemySpawner;
        private readonly Project.Infrastructure.Unity.Enemy.EnemyManager _enemyManager;
        private readonly Project.Infrastructure.Unity.Config.EnemySystemConfig _config;
        private readonly Project.Infrastructure.Unity.Bullet.BulletManager _bulletManager;
        private readonly Project.Infrastructure.Unity.Collision.CollisionManager _collisionManager;

        public EnemyRuntime(
            Project.Infrastructure.Unity.Enemy.EnemyPrefabRegistry prefabRegistry,
            Project.Infrastructure.Unity.Enemy.EnemySpawner enemySpawner,
            Project.Infrastructure.Unity.Enemy.EnemyManager enemyManager,
            Project.Infrastructure.Unity.Config.EnemySystemConfig config,
            Project.Infrastructure.Unity.Bullet.BulletManager bulletManager,
            Project.Infrastructure.Unity.Collision.CollisionManager collisionManager)
        {
            _prefabRegistry = prefabRegistry;
            _enemySpawner = enemySpawner;
            _enemyManager = enemyManager;
            _config = config;
            _bulletManager = bulletManager;
            _collisionManager = collisionManager;
        }

        public void Start()
        {
            if (_config != null)
            {
                if (_config.basicEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Project.Domain.Enemy.EnemyType.Basic, _config.basicEnemyPrefab);
                if (_config.fastEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Project.Domain.Enemy.EnemyType.Fast, _config.fastEnemyPrefab);
                if (_config.tankEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Project.Domain.Enemy.EnemyType.Tank, _config.tankEnemyPrefab);
                if (_config.rangedEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Project.Domain.Enemy.EnemyType.Ranged, _config.rangedEnemyPrefab);
                if (_config.bossEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Project.Domain.Enemy.EnemyType.Boss, _config.bossEnemyPrefab);
            }

            _enemySpawner.SpawnEnemyAtRandomPosition(Project.Domain.Enemy.EnemyType.Basic);
        }

        public void Tick()
        {
            _enemyManager.UpdateAll(Time.deltaTime);
            _bulletManager.Update(Time.deltaTime);
            _collisionManager.Update(Time.deltaTime);
        }
    }
}
