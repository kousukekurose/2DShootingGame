using UnityEngine;
using System;

namespace Project.Application.Player
{
    public class PlayerMoveUseCase
    {
        private readonly Project.Domain.Player.Player _player;
        private readonly Presentation.Player.PlayerView _playerView;

        public PlayerMoveUseCase(Project.Domain.Player.Player player,Presentation.Player.PlayerView playerView)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _playerView = playerView ?? throw new ArgumentNullException(nameof(playerView));
        }

        public void Move(Vector3 direction)
        {
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[PlayerMoveUseCase] Move called with direction: {direction}");
            
            if(!_player.IsActive)
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerMoveUseCase] Player is not active, skipping move");
                return;
            }
            _player.SetInputDirection(direction);
            
            Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerMoveUseCase] Player is active, calling Player.Move");
            _player.Move(direction, Time.deltaTime);
            
            _playerView.UpdatePositionFromPhysics();
        }

        public Vector3 GetCurrentPosition()
        {
            return _player.Position;
        }
    }

}