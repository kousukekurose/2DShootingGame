using System.Collections.Generic;
using UnityEngine;


namespace Project.Infrastructure.Unity.Enemy
{
    public class EnemyPrefabRegistry
    {
        private readonly Dictionary<Project.Domain.Enemy.EnemyType,GameObject> _prefabMap;

        public EnemyPrefabRegistry()
        {
            _prefabMap = new Dictionary<Project.Domain.Enemy.EnemyType, GameObject>();
        }

        public void RegisterPrefab(Project.Domain.Enemy.EnemyType enemyType,GameObject prefab)
        {
            if(prefab == null)
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.LogWarning($"Prefab for {enemyType} is null");
                return;
            }

            if(_prefabMap.ContainsKey(enemyType))
            {
                Project.Infrastructure.Unity.Logging.UnityLogger.LogWarning($"Prefab for {enemyType} already registered, overwriting");
            }

            _prefabMap[enemyType] = prefab;
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"Registered prefab for {enemyType}");
        }

        public GameObject GetPrefab(Project.Domain.Enemy.EnemyType enemyType)
        {
            if(_prefabMap.TryGetValue(enemyType, out var prefab))
            {
                return prefab;
            }

            Project.Infrastructure.Unity.Logging.UnityLogger.LogError($"No prefab registered for {enemyType}");
            return null;
        }

        public bool HasPrefab(Project.Domain.Enemy.EnemyType enemyType)
        {
            return _prefabMap.ContainsKey(enemyType);
        }

    }
}

