using UnityEngine;

namespace Game.Shared.Character
{
    /// <summary>
    /// キャラクターステータスデータのインターフェース
    /// HP、移動速度、攻撃力などのキャラクター基本のステータスを管理
    /// </summary>
    [System.Serializable]
    public class CharacterStats
    {
        [Header("Health")]
        public float MaxHP = 100f;        
        public float CurrentHP;

        [Header("Movement")]
        public float MoveSpeed = 5f;
        public float Acceleration = 10f;
        public float Deceleration = 10f;  

        [Header("Combat")]               
        public float AttackPower = 10f;   
        public float AttackRange = 10f;
        public float AttackCooldown = 0.5f;

        [Header("Defense")]
        public float Defense = 0f;
        public float InvincibilityDuration = 0.5f;

        /// <summary>
        /// HPの割合(0.0~1.0)
        /// </summary>
        public float HPPercentage => CurrentHP / MaxHP;  

        /// <summary>
        /// ステータスを複製する
        /// </summary>
        public CharacterStats Clone()
        {
            return new CharacterStats
            {
                MaxHP = MaxHP,           
                MoveSpeed = MoveSpeed,
                Acceleration = Acceleration,
                Deceleration = Deceleration,  
                AttackPower = AttackPower,    
                AttackRange = AttackRange,
                AttackCooldown = AttackCooldown,
                Defense = Defense,
                InvincibilityDuration = InvincibilityDuration
            };
        }
    }
}