using UnityEngine;

namespace Project.Infrastructure.Unity.Config
{
    [CreateAssetMenu(fileName = "CharacterStatsConfig", menuName = "Game/Character Stats Config")]
    public class CharacterStatsConfig : ScriptableObject
    {
        [Header("Character Stats")]
        public float maxHP = 100f;
        public float moveSpeed = 10f;
        public float attackPower = 10f;
        public float attackRange = 1f;
        public float attackCooldown = 1f;
        public float defense = 0f;

        public Project.Domain.Character.CharacterStats ToDomainStats()
        {
            return new Project.Domain.Character.CharacterStats(
                maxHP,
                moveSpeed,
                attackPower,
                attackRange,
                attackCooldown,
                defense
                );
        }
    }
}
