using UnityEngine;
using MessagePipe;
using R3;

namespace Project.Application.Enemy
{
    public class EnemyAttackUseCase
    {
        private readonly Project.Domain.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;
        private readonly Project.Application.Abstractions.IBulletFactory _bulletFactory;
        private readonly Project.Application.Abstractions.IBulletSpawner _bulletSpawner;
        private readonly ISubscriber<Project.Application.Abstractions.Events.EnemyAttackEvent> _attackEventSubscriber;
        private readonly CompositeDisposable _disposables;

        public EnemyAttackUseCase(
            Project.Domain.Enemy.Enemy enemy,
            Presentation.Enemy.EnemyView enemyView,
            Project.Application.Abstractions.IBulletFactory bulletFactory,
            Project.Application.Abstractions.IBulletSpawner bulletSpawner,
            ISubscriber<Project.Application.Abstractions.Events.EnemyAttackEvent> attackEventSubscriber)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
            _bulletFactory = bulletFactory ?? throw new System.ArgumentNullException(nameof(bulletFactory));
            _bulletSpawner = bulletSpawner ?? throw new System.ArgumentNullException(nameof(bulletSpawner));
            _attackEventSubscriber = attackEventSubscriber ?? throw new System.ArgumentNullException(nameof(attackEventSubscriber));
            _disposables = new CompositeDisposable();

            _attackEventSubscriber.Subscribe(OnEnemyAttack).AddTo(_disposables);
        }

        private void OnEnemyAttack(Project.Application.Abstractions.Events.EnemyAttackEvent attackEvent)
        {
            if(attackEvent.EnemyId != _enemy.CharacterId) return;

            var bullet = _bulletFactory.Create(
                attackEvent.BulletType,
                attackEvent.EnemyId,
                attackEvent.AttackPosition,
                attackEvent.TargetDirection
            );

            _bulletSpawner.SpawnBullet(bullet);
            _enemyView.PlayAnimation("Attack");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
