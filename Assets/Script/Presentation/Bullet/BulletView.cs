using UnityEngine;

namespace Presentation.Bullet
{
    public class BulletView : MonoBehaviour
    {
        private Domain.Bullet.Bullet _bullet;

        public void Initialize(Domain.Bullet.Bullet bullet)
        {
            _bullet = bullet;
            transform.position = bullet.Position;
        }

        private void Update()
        {
            if (_bullet == null)
            {
                return;
            }

            if (!_bullet.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = _bullet.Position;
        }
    }
}
