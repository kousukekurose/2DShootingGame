using UnityEngine;
using System;
using MessagePipe;
using R3;

namespace Project.Application.Player
{
    public class PlayerAttackUseCase
    {
        private readonly Project.Domain.Player.Player _player;
        private readonly Project.Application.Abstractions.IBulletFactory _bulletFactory;
        private readonly Project.Application.Abstractions.IBulletSpawner _bulletSpawner;
        private readonly ISubscriber<Project.Application.Abstractions.Events.PlayerAttackEvent> _attackEventSubscriber;
        private readonly CompositeDisposable _disposables;

        public PlayerAttackUseCase(
            Project.Domain.Player.Player player,
            Project.Application.Abstractions.IBulletFactory bulletFactory,
            Project.Application.Abstractions.IBulletSpawner bulletSpawner,
            ISubscriber<Project.Application.Abstractions.Events.PlayerAttackEvent> attackEventSubscriber)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));
            _bulletSpawner = bulletSpawner ?? throw new ArgumentNullException(nameof(bulletSpawner));
            _attackEventSubscriber = attackEventSubscriber ?? throw new ArgumentNullException(nameof(attackEventSubscriber));
            _disposables = new CompositeDisposable();

            _attackEventSubscriber.Subscribe(OnPlayerAttack).AddTo(_disposables);
        }

        public void Attack(Vector3 targetPosition)
        {
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[PlayerAttackUseCase] Attack called. CanAttack: {_player.CanAttack}");
            if(!_player.CanAttack) 
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerAttackUseCase] Cannot attack - cooldown or inactive");
                return;
            }
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[PlayerAttackUseCase] Calling Player.Attack with target: {targetPosition}");
            _player.Attack(targetPosition, Project.Domain.Bullet.BulletType.Normal);
        }

        private void OnPlayerAttack(Project.Application.Abstractions.Events.PlayerAttackEvent attackEvent)
        {
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"[PlayerAttackUseCase] OnPlayerAttack received. PlayerId: {attackEvent.PlayerId}, MyId: {_player.CharacterId}");
            if(attackEvent.PlayerId != _player.CharacterId) 
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerAttackUseCase] Attack event not for this player");
                return;
            }

            Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerAttackUseCase] Creating bullet");
            var bullet = _bulletFactory.Create(
                attackEvent.BulletType,
                attackEvent.PlayerId,
                attackEvent.AttackPosition,
                attackEvent.TargetDirection
            );

            Project.Infrastructure.Unity.Logging.UnityLogger.Log("[PlayerAttackUseCase] Spawning bullet");
            _bulletSpawner.SpawnBullet(bullet);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
