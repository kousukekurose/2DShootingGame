using UnityEngine;
using Project.Domain.Character;
using Project.Domain.Enemy;

namespace Project.Application.Abstractions.Events
{
    public class EnemyDamageTakenEvent
    {
        public float Damage { get; set; }
        public DamageSource Source {get; set;}
    }

    public class EnemyAttackEvent
    {
        public string EnemyId { get; set; }
        public EnemyType EnemyType { get; set; }
        public Vector3 AttackPosition { get; set; }
        public Vector3 TargetDirection { get; set; }
        public float AttackPower { get; set; }
        public float Timestamp { get; set; }
        public Project.Domain.Bullet.BulletType BulletType { get; set; }
    }
}
