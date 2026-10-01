using Unity.AI.Navigation.LowLevel;
using UnityEngine;
using NumericsVector3 = System.Numerics.Vector3;

namespace Project.Presentation.Bullet
{
    public class BulletView : MonoBehaviour
    {
        private Project.Domain.Bullet.Bullet _bullet;

        public void Initialize(Project.Domain.Bullet.Bullet bullet)
        {
            _bullet = bullet;
            transform.position = ToUnity(_bullet.Position);
        }

        private void Update()
        {

            if (!_bullet.IsAlive || _bullet == null)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.position = ToUnity(_bullet.Position);
        }

        private static Vector3 ToUnity(NumericsVector3 value)
        {
            return new Vector3(value.X,value.Y,value.Z);
        }
    }
}
