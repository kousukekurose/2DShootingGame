using System;
using UnityEngine;

namespace Domain.Bullet
{
    public enum BulletType
    {
        Normal,
        PowerUp,
        Special,
        Explosive
    }

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
            Direction = direction.normalized;
            Speed = spec.Speed;
            Damage = spec.Damage;
            Lifetime = spec.Lifetime;
            IsAlive = true;
        }

        public void Tick(float deltaTime)
        {
            if (!IsAlive)
            {
                return;
            }

            ElapsedTime += deltaTime;
            Position += Direction * Speed * deltaTime;

            if (ElapsedTime >= Lifetime)
            {
                IsAlive = false;
            }
        }
    }
}