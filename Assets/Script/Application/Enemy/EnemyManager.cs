using System.Collections.Generic;

namespace Application.Enemy
{
    public class EnemyManager
    {
        private readonly List<Game.Enemy.Enemy> _activeEnemies = new List<Game.Enemy.Enemy>();
        private readonly Dictionary<int,Game.Enemy.Enemy> _enemyRegistry = new Dictionary<int, Game.Enemy.Enemy>();

        public void RegisterEnemy(Game.Enemy.Enemy enemy)
        {
            _activeEnemies.Add(enemy);
            _enemyRegistry[enemy.GetHashCode()] = enemy;
        }

        public void UnregisterEnemy(Game.Enemy.Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            _enemyRegistry.Remove(enemy.GetHashCode());
        }

        public IEnumerable<Game.Enemy.Enemy> GetActiveEnemies()
        {
            return _activeEnemies;
        }

        public void UpdateAll(float deltaTime)
        {
            foreach(var enemy in _activeEnemies)
            {
                enemy.StateMachine.Update(deltaTime);
            }
        }
    }
}