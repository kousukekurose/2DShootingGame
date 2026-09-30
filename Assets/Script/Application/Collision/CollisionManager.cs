using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Application.Collision
{
    public class CollisionManager
    {
        private readonly Application.Bullet.BulletManager _bulletManager;
        private readonly Application.Enemy.EnemyManager _enemyManager;
        private readonly Game.Player.Player _player;

        public CollisionManager(
            Application.Bullet.BulletManager bulletManager,
            Application.Enemy.EnemyManager enemyManager,
            Game.Player.Player player)
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
                            Framework.Core.CustomLogger.Log($"[CollisionManager] Bullet hit enemy. Enemy HP: {enemy.CurrentHP}/{enemy.MaxHP}, Bullet Damage: {bullet.Damage}");
                            enemy.TakeDamage(bullet.Damage, Framework.Core.Interfaces.DamageSource.Player);
                            Framework.Core.CustomLogger.Log($"[CollisionManager] After damage. Enemy HP: {enemy.CurrentHP}, IsDead: {enemy.IsDead}");
                            bullet.Destroy();
                            break;
                        }
                    }
                }
                // 敵の弾丸 → プレイヤー
                else
                {
                    if(IsCollision(bullet, _player))
                    {
                        Framework.Core.CustomLogger.Log($"[CollisionManager] Enemy bullet hit player. Player HP: {_player.CurrentHP}/{_player.MaxHP}, Bullet Damage: {bullet.Damage}");
                        _player.TakeDamage(bullet.Damage, Framework.Core.Interfaces.DamageSource.Enemy);
                        bullet.Destroy();
                    }
                }
            }
        }

        private bool IsCollision(Domain.Bullet.Bullet bullet, Framework.Core.Interfaces.ITargetable target)
        {
            float distance = Vector3.Distance(bullet.Position, target.Position);
            return distance < 0.5f;
        }
    }
}
