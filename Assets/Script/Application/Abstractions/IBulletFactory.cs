using UnityEngine;

namespace Project.Application.Abstractions
{
    public interface IBulletFactory
    {
        Project.Domain.Bullet.Bullet Create(
            Project.Domain.Bullet.BulletType type,
            string ownerId,
            Vector3 position,
            Vector3 direction);
    }
}
