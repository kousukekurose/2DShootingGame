using UnityEngine;

namespace Project.Infrastructure.Unity.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig",menuName = "Game/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Character Settings")]
        public string CharacterId = "Enemy_001";
        public Project.Domain.Enemy.EnemyType EnemyType = Project.Domain.Enemy.EnemyType.Basic;

        [Header("States")]
        public CharacterStatsConfig DefaultStatsConfig;

        [Header("AI Settings")]
        public float DetectionRange = 10f;
        public float ChaseRange = 5f;  
        public float AttackRange = 2f;
        public float AIUpdateInterval = 0.5f;

        [Header("Behavior")]
        public bool Aggressive = true;
        public bool RetreatOnLowHP = false;
        public float RetreatThreshold = 0.3f;

        [Header("")]
        public string IdleAnimation = "Idle";
        public string ChaseAnimation = "Chase";
        public string AttackAnimation = "Attack";
        public string DamageAnimation = "Damage";
        public string DeathAnimation = "Death"; 

        public Project.Domain.Character.CharacterStats GetDefaultStats()
        {
            return DefaultStatsConfig != null
            ? DefaultStatsConfig.ToDomainStats()
            : new Project.Domain.Character.CharacterStats(100f,5f,10f,10f,0.5f,0f);
        }

    }
}
