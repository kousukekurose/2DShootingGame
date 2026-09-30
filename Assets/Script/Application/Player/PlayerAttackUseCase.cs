using UnityEngine;
using System;
using MessagePipe;
using R3;

namespace Project.Application.Player
{
    public class PlayerAttackUseCase
    {
        private readonly Project.Game.Player.Player _player;
        private readonly Bullet.IBulletFactory _bulletFactory;
        private readonly Bullet.BulletManager _bulletManager;
        private readonly ISubscriber<Framework.Core.Events.PlayerAttackEvent> _attackEventSubscriber;
        private readonly CompositeDisposable _disposables;

        public PlayerAttackUseCase(
            Project.Game.Player.Player player,
            Bullet.IBulletFactory bulletFactory,
            Bullet.BulletManager bulletManager,
            ISubscriber<Framework.Core.Events.PlayerAttackEvent> attackEventSubscriber)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
            _bulletManager = bulletManager ?? throw new ArgumentNullException(nameof(bulletManager));
            _attackEventSubscriber = attackEventSubscriber ?? throw new ArgumentNullException(nameof(attackEventSubscriber));
            _disposables = new CompositeDisposable();

            _attackEventSubscriber.Subscribe(OnPlayerAttack).AddTo(_disposables);
        }

        public void Attack(Vector3 targetPosition)
        {
            Framework.Core.CustomLogger.Log($"[PlayerAttackUseCase] Attack called. CanAttack: {_player.CanAttack}");
            if(!_player.CanAttack) 
            {
                Framework.Core.CustomLogger.Log("[PlayerAttackUseCase] Cannot attack - cooldown or inactive");
                return;
            }
            Framework.Core.CustomLogger.Log($"[PlayerAttackUseCase] Calling Player.Attack with target: {targetPosition}");
            _player.Attack(targetPosition, Domain.Bullet.BulletType.Normal);
        }

        private void OnPlayerAttack(Framework.Core.Events.PlayerAttackEvent attackEvent)
        {
            Framework.Core.CustomLogger.Log($"[PlayerAttackUseCase] OnPlayerAttack received. PlayerId: {attackEvent.PlayerId}, MyId: {_player.CharacterId}");
            if(attackEvent.PlayerId != _player.CharacterId) 
            {
                Framework.Core.CustomLogger.Log("[PlayerAttackUseCase] Attack event not for this player");
                return;
            }

            Framework.Core.CustomLogger.Log("[PlayerAttackUseCase] Creating bullet");
            var bullet = _bulletFactory.Create(
                attackEvent.BulletType,
                attackEvent.PlayerId,
                attackEvent.AttackPosition,
                attackEvent.TargetDirection
            );

            Framework.Core.CustomLogger.Log("[PlayerAttackUseCase] Spawning bullet");
            _bulletManager.SpawnBullet(bullet);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
