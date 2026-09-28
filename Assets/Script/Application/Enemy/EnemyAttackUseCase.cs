using UnityEngine;

namespace Application.Enemy
{
    public class EnemyAttackUseCase
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;
        private readonly Bullet.BulletFactory _bulletFactory;
        private readonly Bullet.BulletManager _bulletManager;

        public EnemyAttackUseCase(
            Game.Enemy.Enemy enemy,
            Presentation.Enemy.EnemyView enemyView,
            Bullet.BulletFactory bulletFactory,
            Bullet.BulletManager bulletManager
        )
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
            _bulletFactory = bulletFactory ?? throw new System.ArgumentNullException(nameof(bulletFactory));
            _bulletManager = bulletManager ?? throw new System.ArgumentNullException(nameof(bulletManager));
        }

        public void Attack(Vector3 targetPosition)
        {
            if(!_enemy.CanAttack) return;
            var direction = (targetPosition - _enemy.GetCurrentPosition()).normalized;
            var bullet = _bulletFactory.Create(
                Domain.Bullet.BulletType.Normal,
                _enemy.CharacterId,
                _enemy.GetCurrentPosition(),
                direction
            );
            _bulletManager.SpawnBullet(bullet);
            _enemy.Attack(targetPosition, Domain.Bullet.BulletType.Normal);
            _enemyView.PlayAnimation("Attack");
        }
    }
}
