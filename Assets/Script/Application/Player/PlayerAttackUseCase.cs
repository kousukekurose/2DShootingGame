using UnityEngine;
using System;

namespace Application.Player
{
    public class PlayerAttackUseCase
    {
        private readonly Game.Player.Player _player;
        private readonly Presentation.Player.PlayerView _playerView;

        public PlayerAttackUseCase(Game.Player.Player player,Presentation.Player.PlayerView playerView)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _playerView = playerView ?? throw new ArgumentNullException(nameof(playerView));
        }

        public void Attack(Vector3 targetPosition)
        {
            if(!_player.CanAttack) return;
            _player.Attack(targetPosition);
            //_playerView.PlayAnimation("Attack");
        }
    }
}
