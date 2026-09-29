using UnityEngine;

namespace Framework.Core.Events
{
    public class EnemyStateChangedEvent
    {
        public string StateName { get; set; }
    }

    public class EnemyDamageTakenEvent
    {
        public float Damage { get; set; }
        public Interfaces.DamageSource Source {get; set;}
    }

    public class EnemyAttackEvent
    {
        public string EnemyId { get; set; }
        public Interfaces.EnemyType EnemyType { get; set; }
        public Vector3 AttackPosition { get; set; }
        public Vector3 TargetDirection { get; set; }
        public float AttackPower { get; set; }
        public float Timestamp { get; set; }
        public Domain.Bullet.BulletType BulletType { get; set; }
    }
}
