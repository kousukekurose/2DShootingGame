using UnityEngine;

namespace Framework.Core.Interfaces
{
    public interface IBullet
    {
        void Initialize(string ownerId, float attackPower, Vector3 direction, int bulletId,Vector3 initialPosition,float speed);
        void UpdatePosition(float deltaTime);
        void Destroy();
    }
}