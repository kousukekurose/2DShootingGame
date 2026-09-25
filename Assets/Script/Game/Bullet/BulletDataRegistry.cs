using System.Collections.Generic;
using UnityEngine;

namespace Game.Bullet
{
    public class BulletDataRegistry
    {
        private readonly Dictionary<Domain.Bullet.BulletType, BulletData> _bulletDataMap;

        public BulletDataRegistry(BulletData[] bulletDataArray)
        {
            _bulletDataMap = new Dictionary<Domain.Bullet.BulletType, BulletData>();

            if (bulletDataArray == null)
            {
                return;
            }

            foreach (var data in bulletDataArray)
            {
                if (data == null)
                {
                    continue;
                }

                if (_bulletDataMap.ContainsKey(data.Type))
                {
                    Debug.LogWarning($"Duplicate bullet data for type: {data.Type}. Overwriting.");
                }

                _bulletDataMap[data.Type] = data;
            }
        }

        public BulletData GetData(Domain.Bullet.BulletType type)
        {
            return _bulletDataMap.TryGetValue(type, out var data) ? data : null;
        }
    }
}

