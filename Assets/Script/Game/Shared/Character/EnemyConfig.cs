using Framework.Core.Interfaces;
using UnityEngine;

namespace Game.Shared.Character
{
    [CreateAssetMenu(fileName = "EnemyConfig",menuName = "Game/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Character Settings")]
        public string CharacterId = "Enemy_001";
        public EnemyType EnemyType = EnemyType.Basic;

        [Header("States")]
        public CharacterStats DefaultStats;

        [Header("AI Settings")]
        public float DetectionRange = 10f;
        public float ChaseRange = 5f;  
        public float AttackRange = 2f;
        public float AIUpdateInterval = 0.5f;

        [Header("Behavior")]
        public bool Aggressive = true;
        public bool RetreatOnLowHP = false;
        public float RetreatThresholod = 0.3f;

    }
}
