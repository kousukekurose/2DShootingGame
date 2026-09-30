using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Application.Bullet
{
    public class BulletManager
    {
        private readonly List<Domain.Bullet.Bullet> _activeBullets = new List<Domain.Bullet.Bullet>();
        private readonly Dictionary<Domain.Bullet.Bullet,Presentation.Bullet.BulletView> _bulletViewMap = new Dictionary<Domain.Bullet.Bullet, Presentation.Bullet.BulletView>();
        private readonly Game.Bullet.BulletDataRegistry _bulletDataRegistry;
        //オブジェクトプール
        private readonly Dictionary<Domain.Bullet.BulletType,ObjectPool<GameObject>> _bulletViewPools = new Dictionary<Domain.Bullet.BulletType, ObjectPool<GameObject>>();
        private Transform _bulletRootFolder;
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
                var bulletView = GetBulletViewFromPool(bullet.Type,bulletData.ViewPrefab);
                bulletView.transform.position = bullet.Position;
                bulletView.Initialize(bullet);
                _bulletViewMap[bullet] = bulletView;
            }
        }

        private Presentation.Bullet.BulletView GetBulletViewFromPool(Domain.Bullet.BulletType bulletType,GameObject prefab)
        {
            if(!_bulletViewPools.ContainsKey(bulletType))
            {
                if(_bulletRootFolder == null)
                {
                    _bulletRootFolder = new GameObject("BulletFolder").transform;
                }
                //プールがなければ作成
                _bulletViewPools[bulletType] = new ObjectPool<GameObject>(
                    createFunc: () => GameObject.Instantiate(prefab,_bulletRootFolder),
                    actionOnGet: view =>
                    {
                        view.SetActive(true);
                    },
                    actionOnRelease: view =>
                    {
                        view.SetActive(false);
                    },
                    actionOnDestroy: view =>
                    {
                        GameObject.Destroy(view);
                    },
                    maxSize: 100
                );
            }
            var bullerViewObject = _bulletViewPools[bulletType].Get();
            return bullerViewObject.GetComponent<Presentation.Bullet.BulletView>();
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
                    ReturnBullerViewToPool(bullet.Type,view.gameObject);
                }
                _bulletViewMap.Remove(bullet);
            }
        }

        private void ReturnBullerViewToPool(Domain.Bullet.BulletType bulletType,GameObject bulletView)
        {
            if(_bulletViewPools.ContainsKey(bulletType))
            {
                _bulletViewPools[bulletType].Release(bulletView);
            }
        }

        public IEnumerable<Domain.Bullet.Bullet> GetActiveBullets()
        {
            return _activeBullets;
        }
    }
}