using System;

namespace Project.Domain.Bullet
{
    public sealed class BulletSpec
    {
        public float Speed { get; }
        public float Damage { get; }
        public float Lifetime { get; }

        public BulletSpec(float speed, float damage, float lifetime)
        {
            if (speed <= 0f) throw new ArgumentOutOfRangeException(nameof(speed));
            if (damage < 0f) throw new ArgumentOutOfRangeException(nameof(damage));
            if (lifetime <= 0f) throw new ArgumentOutOfRangeException(nameof(lifetime));

            Speed = speed;
            Damage = damage;
            Lifetime = lifetime;
        }
    }
}
