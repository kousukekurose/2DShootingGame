using R3;

namespace Domain.Character
{
    public interface ICharacter
    {
        ReadOnlyReactiveProperty<System.Numerics.Vector3> Position { get; }
        ReadOnlyReactiveProperty<float> CurrentHP { get; }
        bool IsDead { get; }
        void TakeDamage(float damage);
    }
}