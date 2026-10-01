using System;
using System.Numerics;

namespace Project.Domain.Bullet
{
    public sealed class Bullet
    {
        public BulletType Type { get; }
        public string OwnerId { get; }
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; }
        public float Speed { get; }
        public float Damage { get; }
        public float Lifetime { get; }
        public float ElapsedTime { get; private set; }
        public bool IsAlive { get; private set; }

        public Bullet(
            BulletType type,
            string ownerId,
            Vector3 position,
            Vector3 direction,
            BulletSpec spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            Type = type;
            OwnerId = ownerId;
            Position = position;
            Direction = direction.LengthSquared() > 0f
                ? Vector3.Normalize(direction)
                : Vector3.Zero;
            Speed = spec.Speed;
            Damage = spec.Damage;
            Lifetime = spec.Lifetime;
            IsAlive = true;
        }

        public void Tick(float deltaTime)
        {
            if (!IsAlive) return;

            ElapsedTime += deltaTime;
            Position += Direction * Speed * deltaTime;

            if (ElapsedTime >= Lifetime)
            {
                IsAlive = false;
            }
        }

        public void Destroy()
        {
            IsAlive = false;
        }
    }
}
