
namespace Domain.Character
{
    /// <summary>
    /// ステータスを複製する
     /// </summary>
    public class CharacterStats
    {
        public float MaxHP { get; }
        public float MoveSpeed { get; }
        public float AttackPower { get; }
        public float AttackRange { get; }
        public float AttackCooldown { get; }
        public float Defense { get; }

        public CharacterStats(
            float maxHP,
            float moveSpeed,
            float attackPower,
            float attackRange,
            float attackCooldown,
            float defense
        )
        {
            MaxHP = maxHP;
            MoveSpeed = moveSpeed;
            AttackPower = attackPower;
            AttackRange = attackRange;
            AttackCooldown = attackCooldown;
            Defense = defense;
        }

        public enum StatModifilerType { Flat, Precent }

        public class StatModifiler
        {
            public float Value { get; }
            public StatModifilerType Type { get; }
            public object Source { get; }

            public StatModifiler(float value,StatModifilerType type, object source)
            {
                Value = value;
                Type = type;
                Source = source;
            }
        }
    }
    
}