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

            if (!_bullet.IsAlive || _bullet == null)
            {
                gameObject.SetActive(false);
                return;
            }
            //_bullet.Tick(Time.deltaTime);

            transform.position = _bullet.Position;
        }
    }
}
