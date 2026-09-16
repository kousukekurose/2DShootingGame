using System.Collections.Generic;
using UnityEngine;


namespace Application.Enemy
{
    public class EnemyPrefabRegistry
    {
        private readonly Dictionary<Framework.Core.Interfaces.EnemyType,GameObject> _prefabMap;

        public EnemyPrefabRegistry()
        {
            _prefabMap = new Dictionary<Framework.Core.Interfaces.EnemyType, GameObject>();
        }

        public void RegisterPrefab(Framework.Core.Interfaces.EnemyType enemyType,GameObject prefab)
        {
            if(prefab == null)
            {
                Framework.Core.CustomLogger.LogWarning($"Prefab for {enemyType} is null");
                return;
            }

            if(_prefabMap.ContainsKey(enemyType))
            {
                Framework.Core.CustomLogger.LogWarning($"Prefab for{enemyType} already registered,overwrting");
            }

            _prefabMap[enemyType] = prefab;
            Framework.Core.CustomLogger.Log($"Registered prefab for {enemyType}");
        }

        public GameObject GetPrefab(Framework.Core.Interfaces.EnemyType enemyType)
        {
            if(_prefabMap.TryGetValue(enemyType, out var prefab))
            {
                return prefab;
            }

            Framework.Core.CustomLogger.LogError($"Vo prefab registered for {enemyType}");
            return null;
        }

        public bool HasPrefab(Framework.Core.Interfaces.EnemyType enemyType)
        {
            return _prefabMap.ContainsKey(enemyType);
        }

    }
}

