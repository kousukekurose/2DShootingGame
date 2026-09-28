using UnityEngine;
using System;

namespace Application.Player
{
    public class PlayerAttackUseCase
    {
        private readonly Game.Player.Player _player;
        private readonly Bullet.IBulletFactory _bulletFactory;
        private readonly Application.Bullet.BulletManager _bulletManager;

        public PlayerAttackUseCase(Game.Player.Player player,Bullet.IBulletFactory bulletFactory,Application.Bullet.BulletManager bulletManager)
        {
            _player = player;
            _bulletFactory = bulletFactory;
            _bulletManager = bulletManager;
        }

        public void Attack(Vector3 targetPosition)
        {
            if(!_player.CanAttack) return;
            var direction = (targetPosition - _player.Position).normalized;
            var bullet = _bulletFactory.Create(
                Domain.Bullet.BulletType.Normal,
                _player.CharacterId,
                _player.Position,
                direction
            );
            
            _bulletManager.SpawnBullet(bullet);
            _player.Attack(targetPosition,Domain.Bullet.BulletType.Normal);
        }
    }
}
