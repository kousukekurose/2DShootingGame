
using UnityEngine;
using Project.Domain.Character;
using Project.Infrastructure.Unity.Config;

namespace Project.Application.Abstractions.Events
{
    public class PlayerDamageTakenEvent
    {
        public float Damage { get; set; }
        public DamageSource Source {get; set;}
        public float CurrentHP {get; set;}
        public float MaxHP{get; set;}
    }

    public class PlayerMoveEvent
    {
        public Vector3 PreviousPosition {get; set;}
        public Vector3 Newposition {get; set;}
    }

    public class PlayerAttackEvent
    {
        public string PlayerId{get; set; }
        public Vector3 AttackPosition{get; set; }
        public Vector3 TargetDirection {get; set; }
        public float AttackPower {get; set; }
        public float Timestamp {get; set; }
        public int BulletId{get; set;}
        public Project.Domain.Bullet.BulletType BulletType { get; set; } 
    } 

    public class PlayerDeathEvent
    {
        public string PlayerId{get; set;}
        public Vector3 DeathPosition {get; set;}
    }
}
