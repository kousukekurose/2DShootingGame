using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Application.Player
{
    public class PlayerDamageUseCase
    {
        private readonly Game.Player.Player _player;
        private readonly Presentation.Player.PlayerView _playerView;
        private bool _isInvincibleTaskRunning = false;

        public PlayerDamageUseCase(Game.Player.Player player,Presentation.Player.PlayerView playerView)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _playerView = playerView ?? throw new ArgumentNullException(nameof(playerView));
        }

        public void TakeDamage(float damage,Framework.Core.Interfaces.DamageSource source)
        {
            if(_player.IsDead || _player.IsInvincible) return;
            _player.TakeDamage(damage,source);
            _playerView.SetColor(Color.red);
            ResetColorAfterDelay().Forget();
            if(_player.IsDead)
            {
                OnPlayerDeath();
            }
        }

        public void Heal(float amount)
        {
            if(_player.IsDead) return;
            _player.Heal(amount);
            _playerView.SetColor(Color.green);
            ResetColorAfterDelay().Forget();
        }

        public async UniTask SetInvincible(float duration,CancellationToken cancellationToken = default)
        {
            if(_isInvincibleTaskRunning) return;
            _isInvincibleTaskRunning = true;
            _player.SetInvincible();
            _playerView.SetColor(Color.blue);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
            }
            catch(OperationCanceledException)
            {
                return;
            }
            finally
            {
                _player.ClearInvincible();
                _playerView.SetColor(Color.white);
                _isInvincibleTaskRunning = false;
            }
        }

        public void ActivatePlayer()
        {
            _player.Activate();
            _playerView.SetVisible(true);
        }

        public void DeactivatePlayer()
        {
            _player.Deactivate();
            _playerView.SetVisible(false);
        }

        public void Update()
        {
            if(!_player.IsActive) return;
            _player.UpdateCooldownTimer(Time.deltaTime);
            _player.StateMachine?.Update(Time.deltaTime);
        }

        private void OnPlayerDeath()
        {
            _playerView.SetVisible(false);
        }

        private async UniTaskVoid ResetColorAfterDelay()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                _playerView.SetColor(Color.white);
            }
            catch(OperationCanceledException){}
        }
    }
}
