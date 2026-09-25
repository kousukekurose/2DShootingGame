using System;
using UnityEngine;

namespace Application.Bullet
{
    public interface IBulletFactory
    {
        Domain.Bullet.Bullet Create(
            Domain.Bullet.BulletType type,
            string ownerId,
            Vector3 position,
            Vector3 direction);
    }

    public sealed class BulletFactory : IBulletFactory
    {
        private readonly Game.Bullet.BulletDataRegistry _registry;

        public BulletFactory(Game.Bullet.BulletDataRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public Domain.Bullet.Bullet Create(
            Domain.Bullet.BulletType type,
            string ownerId,
            Vector3 position,
            Vector3 direction)
        {
            var data = _registry.GetData(type);
            if (data == null)
            {
                throw new InvalidOperationException($"Bullet data not found for type: {type}");
            }

            var spec = new Domain.Bullet.BulletSpec(
                data.Speed,
                data.Damage,
                data.Lifetime);

            return new Domain.Bullet.Bullet(type, ownerId, position, direction, spec);
        }
    }
}
