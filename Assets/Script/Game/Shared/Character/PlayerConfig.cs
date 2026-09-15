using Framework.Core.Patterns;
using UnityEngine;

namespace Game.Shared.Character
{
    [CreateAssetMenu(fileName = "PlayerConfig",menuName = "Game/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Character Settings")]
        public string CharacterId = "Player001";

        [Header("States")]
        public CharacterStats DefaultStats;

        [Header("GamePlay")]
        public float MoveSpeed = 5f;
        public float AttackRAnge = 5f;
        public float AttackCooldown = 0.5f;
        public float AttackPower = 10f;  
        public float Defense = 0f;
        public float MaxHP = 100f;  

        [Header("Visual")]
        public string IdleAnimation = "Idle";
        public string MoveAnimation = "Move";
        public string AttackAnimation = "Attack";
        public string DamageAnimation = "Damage";
        public string DeathAnimation = "Death";  

    }
}