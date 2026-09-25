using UnityEngine;

namespace Application.Enemy
{
    public class EnemyAttackUseCase
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;

        public EnemyAttackUseCase(Game.Enemy.Enemy enemy,Presentation.Enemy.EnemyView enemyView)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
        }

        public void Attack(Vector3 targetPosition)
        {
            if(!_enemy.CanAttack) return;
            _enemy.Attack(targetPosition, Domain.Bullet.BulletType.Normal);
            _enemyView.PlayAnimation("Attack");
        }
    }
}
