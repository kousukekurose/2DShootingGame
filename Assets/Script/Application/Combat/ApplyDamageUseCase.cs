using Project.Domain.Character;

namespace Project.Application.Combat
{
    public sealed class ApplyDamageUseCase
    {
        public void Execute(IDamageable target, float damage, DamageSource source)
        {
            if (target == null || !target.IsActive || target.IsDead) return;
            target.TakeDamage(damage, source);
        }
    }
}
