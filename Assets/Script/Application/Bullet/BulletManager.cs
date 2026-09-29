using System.Collections.Generic;
using Cysharp.Threading.Tasks.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Application.Bullet
{
    public class BulletManager
    {
        private readonly List<Domain.Bullet.Bullet> _activeBullets = new List<Domain.Bullet.Bullet>();
        private readonly Dictionary<Domain.Bullet.Bullet,Presentation.Bullet.BulletView> _bulletViewMap = new Dictionary<Domain.Bullet.Bullet, Presentation.Bullet.BulletView>();
        private readonly Game.Bullet.BulletDataRegistry _bulletDataRegistry;

        public BulletManager(Game.Bullet.BulletDataRegistry bulletDataRegistry)
        {
            _bulletDataRegistry = bulletDataRegistry;
        }

        public void SpawnBullet(Domain.Bullet.Bullet bullet)
        {
            _activeBullets.Add(bullet);

            var bulletData = _bulletDataRegistry.GetData(bullet.Type);
            if(bulletData != null && bulletData.ViewPrefab != null)
            {
                var bulletViewObject =  GameObject.Instantiate(bulletData.ViewPrefab,bullet.Position,Quaternion.identity);
                var bulletView = bulletViewObject.GetComponent<Presentation.Bullet.BulletView>();
                if(bulletView != null)
                {
                    bulletView.Initialize(bullet);
                    _bulletViewMap[bullet] = bulletView;
                }
                else
                {
                    GameObject.Destroy(bulletViewObject);
                }
            }
        }

        public void Update(float deltaTime) 
        {
            for(int i = _activeBullets.Count -1; i >= 0;i--)
            {
                var bullet = _activeBullets[i];
                bullet.Tick(deltaTime);

                if(_bulletViewMap.TryGetValue(bullet, out var view))
                {
                    if(view != null)
                    {
                        view.transform.position = bullet.Position;
                    }
                }

                if(!bullet.IsAlive)
                {
                    RemoveBullet(bullet);
                }
            }
        }

        private void RemoveBullet(Domain.Bullet.Bullet bullet)
        {
            _activeBullets.Remove(bullet);
            if(_bulletViewMap.TryGetValue(bullet, out var view))
            {
                if(view != null)
                {
                    GameObject.Destroy(view.gameObject);
                }
                _bulletViewMap.Remove(bullet);
            }
        }

        public IEnumerable<Domain.Bullet.Bullet> GetActiveBullets()
        {
            return _activeBullets;
        }
    }
}