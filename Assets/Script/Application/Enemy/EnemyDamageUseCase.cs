using UnityEngine;


namespace Application.Enemy
{
    public class EnemyDamageUseCase
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;

        public EnemyDamageUseCase(Game.Enemy.Enemy enemy,Presentation.Enemy.EnemyView enemyView)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
        }

        public void TakeDamage(float damage,Framework.Core.Interfaces.DamageSource source)
        {
            if(!_enemy.IsActive)return;
            _enemy.TakeDamage(damage,source);
            //_enemyView.PlayAnimation("Damage");
            _enemyView.SetColor(Color.red);
        }

        public void Update()
        {
            _enemy.UpdateCooldownTimer(Time.deltaTime);
        }
    }
}
