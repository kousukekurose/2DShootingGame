using UnityEngine;
using MessagePipe;
using R3;

namespace Project.Application.Enemy
{
    public class EnemyAttackUseCase
    {
        private readonly Project.Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;
        private readonly Bullet.IBulletFactory _bulletFactory;
        private readonly Bullet.BulletManager _bulletManager;
        private readonly ISubscriber<Framework.Core.Events.EnemyAttackEvent> _attackEventSubscriber;
        private readonly CompositeDisposable _disposables;

        public EnemyAttackUseCase(
            Project.Game.Enemy.Enemy enemy,
            Presentation.Enemy.EnemyView enemyView,
            Bullet.IBulletFactory bulletFactory,
            Bullet.BulletManager bulletManager,
            ISubscriber<Framework.Core.Events.EnemyAttackEvent> attackEventSubscriber)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
            _bulletFactory = bulletFactory ?? throw new System.ArgumentNullException(nameof(bulletFactory));
            _bulletManager = bulletManager ?? throw new System.ArgumentNullException(nameof(bulletManager));
            _attackEventSubscriber = attackEventSubscriber ?? throw new System.ArgumentNullException(nameof(attackEventSubscriber));
            _disposables = new CompositeDisposable();

            _attackEventSubscriber.Subscribe(OnEnemyAttack).AddTo(_disposables);
        }

        private void OnEnemyAttack(Framework.Core.Events.EnemyAttackEvent attackEvent)
        {
            if(attackEvent.EnemyId != _enemy.CharacterId) return;

            var bullet = _bulletFactory.Create(
                attackEvent.BulletType,
                attackEvent.EnemyId,
                attackEvent.AttackPosition,
                attackEvent.TargetDirection
            );

            _bulletManager.SpawnBullet(bullet);
            _enemyView.PlayAnimation("Attack");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
