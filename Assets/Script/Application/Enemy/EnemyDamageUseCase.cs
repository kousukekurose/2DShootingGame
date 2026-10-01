
namespace Project.Application.Enemy
{
    public class EnemyDamageUseCase
    {
        private readonly Project.Domain.Character.IDamageable _target;
        public EnemyDamageUseCase(Project.Domain.Character.IDamageable target)
        {
            _target = target;
        }

        public void TakeDamage(float damage,Project.Domain.Character.DamageSource source)
        {
            if(!_target.IsActive || _target.IsDead)return;
            _target.TakeDamage(damage,source);
        }
    }
}
