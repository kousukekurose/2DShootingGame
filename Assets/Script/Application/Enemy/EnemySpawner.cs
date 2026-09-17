using UnityEngine;
using VContainer;
using MessagePipe;

namespace Application.Enemy
{
    public class EnemySpawner
    {
        private readonly EnemyFactory  _enemyFactory;
        private Framework.Core.Interfaces.ITargetable _playerTarget;

        public EnemySpawner(
            EnemyFactory enemyFactory,
            Framework.Core.Interfaces.ITargetable playerTarget
        )
        {
            _enemyFactory = enemyFactory;
            _playerTarget = playerTarget;
        }

        public Presentation.Enemy.EnemyView SpawnEnemyAtRandomPosition(
            Framework.Core.Interfaces.EnemyType enemyType,
            float minDistance = 5f,
            float maxDistance = 8f
        )
        {
            float screenHalfWidth = 3.5f;
            float randomX = Random.Range(-screenHalfWidth,screenHalfWidth);
            float randomY = Random.Range(minDistance,maxDistance);

            var offset = new Vector3(randomX,randomY,0f);

            Vector3 playerPosition = _playerTarget.Position;

            var spawnPosition = playerPosition + offset;

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