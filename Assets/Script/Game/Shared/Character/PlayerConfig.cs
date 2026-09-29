using UnityEngine;

namespace Game.Shared.Character
{
    [CreateAssetMenu(fileName = "PlayerConfig",menuName = "Game/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Character Settings")]
        public string CharacterId = "Player001";

        [Header("States")]
        public CharacterStatsConfig DefaultStatsConfig;

        [Header("Animation Names")]
        public string IdleAnimation = "Idle";
        public string MoveAnimation = "Move";
        public string AttackAnimation = "Attack";
        public string DamageAnimation = "Damage";
        public string DeathAnimation = "Death";  

        public Domain.Character.CharacterStats GetDefaultStats()
        {
            return DefaultStatsConfig != null
            ? DefaultStatsConfig.ToDomainStats()
            : new Domain.Character.CharacterStats(100f,5f,10f,10f,0.5f,0f);
        }

    }
}