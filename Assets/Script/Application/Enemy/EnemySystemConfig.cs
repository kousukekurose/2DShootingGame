using UnityEngine;

namespace Application.Enemy
{
    [CreateAssetMenu(fileName ="EnemySystemConfig",menuName ="Game/Enemy Sysytem Config")]
    public class EnemySystemConfig : ScriptableObject
    {
        [Header("Enemy Prefab")]
        public GameObject basicEnemyPrefab;
        public GameObject fastEnemyPrefab;
        public GameObject tankEnemyPrefab;
        public GameObject rangedEnemyPrefab;
        public GameObject bossEnemyPrefab;
    }
}

