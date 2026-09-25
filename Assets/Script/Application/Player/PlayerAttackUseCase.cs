using UnityEngine;
using System;

namespace Application.Player
{
    public class PlayerAttackUseCase
    {
        private readonly Game.Player.Player _player;
        private readonly Bullet.IBulletFactory _bulletFactory;

        public PlayerAttackUseCase(Game.Player.Player player,Bullet.IBulletFactory bulletFactory)
        {
            _player = player;
            _bulletFactory = bulletFactory;
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
            
            _player.Attack(targetPosition,Domain.Bullet.BulletType.Normal);
        }
    }
}
