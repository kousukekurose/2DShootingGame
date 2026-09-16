using UnityEngine;
using VContainer;
using MessagePipe;

namespace Application.Enemy
{
    public class EnemySpawner
    {
        private readonly EnemyFactory  _enemyFactory;
        private Game.Shared.Character.PlayerConfig  _playerConfig;

        public EnemySpawner(
            EnemyFactory enemyFactory,
            Game.Shared.Character.PlayerConfig playerConfig
        )
        {
            _enemyFactory = enemyFactory;
            _playerConfig = playerConfig;
        }

        public Presentation.Enemy.EnemyView SpawnEnemyAtRandomPosition(
            Framework.Core.Interfaces.EnemyType enemyType,
            float minDistance = 5f,
            float maxDistance = 15f
        )
        {
            var randomAngle = Random.Range(0,360f) * Mathf.Deg2Rad;
            var randomDistance = Random.Range(minDistance,maxDistance);

            var spawnPosition = new Vector3(
                Mathf.Cos(randomAngle) * randomDistance,
                Mathf.Sin(randomAngle) * randomDistance,
                0f
            );

            return _enemyFactory.SpawnEnemy(spawnPosition,enemyType);
        }

        public Presentation.Enemy.EnemyView SpawnEnemyAtPosition(
            Vector3 position,
            Framework.Core.Interfaces.EnemyType enemyType
        )
        {
            return _enemyFactory.SpawnEnemy(position,enemyType);
        }

        public void SpawnWave(
            Framework.Core.Interfaces.EnemyType enemyType,
            int count,
            float minDistance = 5f,
            float maxDistance = 15f
        )
        {
            for(int i = 0; i < count; i++)
            {
                SpawnEnemyAtRandomPosition(enemyType,minDistance,maxDistance);
            }
        }
    }
}