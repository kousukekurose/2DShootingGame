using System;
using UnityEngine;
using NumericsVector3 = System.Numerics.Vector3;

namespace Project.Infrastructure.Unity.Bullet
{
    
    public sealed class BulletFactory : Project.Application.Abstractions.IBulletFactory
    {
        private readonly Project.Infrastructure.Unity.Config.BulletDataRegistry _registry;

        public BulletFactory(Project.Infrastructure.Unity.Config.BulletDataRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public Project.Domain.Bullet.Bullet Create(
            Project.Domain.Bullet.BulletType type,
            string ownerId,
            Vector3 position,
            Vector3 direction)
        {
            var data = _registry.GetData(type);
            if (data == null)
            {
                throw new InvalidOperationException($"Bullet data not found for type: {type}");
            }

            var spec = new Project.Domain.Bullet.BulletSpec(
                data.Speed,
                data.Damage,
                data.Lifetime);

            return new Project.Domain.Bullet.Bullet(type, ownerId, ToNumerics(position), ToNumerics(direction), spec);
        }

        private static NumericsVector3 ToNumerics(Vector3 value)
        {
            return new NumericsVector3(value.x,value.y,value.z);
        }
    }
}
