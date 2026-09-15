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
        public float MaxHP = 100f;        
        public float MoveSpeed = 5f;          
        public float AttackPower = 10f;   
        public float AttackRange = 10f;
        public float AttackCooldown = 0.5f;
        public float Defense = 0f;

        /// <summary>
        /// ステータスを複製する
        /// </summary>
        public CharacterStats Clone()
        {
            return new CharacterStats
            {
                MaxHP = MaxHP,   
                MoveSpeed = MoveSpeed,  
                AttackPower = AttackPower,    
                AttackRange = AttackRange,
                AttackCooldown = AttackCooldown,
                Defense = Defense,
            };
        }
    }
}