using System.Linq;
using Project.Domain.Character;
using NumericsVector3 = System.Numerics.Vector3;

namespace Project.Infrastructure.Unity.Collision
{
    public class CollisionManager
    {
        private readonly Project.Infrastructure.Unity.Bullet.BulletManager _bulletManager;
        private readonly Project.Infrastructure.Unity.Enemy.EnemyManager _enemyManager;
        private readonly Project.Domain.Player.Player _player;

        public CollisionManager(
            Project.Infrastructure.Unity.Bullet.BulletManager bulletManager,
            Project.Infrastructure.Unity.Enemy.EnemyManager enemyManager,
            Project.Domain.Player.Player player)
        {
            _bulletManager = bulletManager;
            _enemyManager = enemyManager;
            _player = player;
        }

        public void Update(float deltaTime)
        {
            CheckBulletCollisions();
        }

        private void CheckBulletCollisions()
        {
            var activeBullets = _bulletManager.GetActiveBullets().ToList();
            
            foreach(var bullet in activeBullets)
            {
                if(!bullet.IsAlive) continue;

                // プレイヤーの弾丸 → 敵
                if(bullet.OwnerId == _player.CharacterId)
                {
                    foreach(var enemy in _enemyManager.GetActiveEnemies())
                    {
                        if(!enemy.IsActive || enemy.IsDead) continue;
                        if(IsCollision(bullet, enemy))
                        {
                            enemy.TakeDamage(bullet.Damage, DamageSource.Player);
                            bullet.Destroy();
                            break;
                        }
                    }
                }
                // 敵の弾丸 → プレイヤー
                else if(IsCollision(bullet, _player))
                {
                    _player.TakeDamage(bullet.Damage, DamageSource.Enemy);
                    bullet.Destroy();
                }
            }
        }

        private bool IsCollision(Project.Domain.Bullet.Bullet bullet,ITargetable target)
        {
            var targetPosition = target.Position;
            var targetPositionValue = new NumericsVector3(
                targetPosition.x,
                targetPosition.y,
                targetPosition.z
            );
            return NumericsVector3.Distance(
                bullet.Position,
                targetPositionValue
            ) < 0.5f;
        }
    }
}
