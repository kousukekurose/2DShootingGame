
using UnityEngine;
using Framework.Core.Interfaces;
using Game.Player.PlayerState;
using Game.Bullet;

namespace Framework.Core.Events
{
    public class PlayerStateChangedEvent
    {
        public string StateName { get; set; }
        public PlayerState State {get; set;}
        public float Timestamp {get; set;}
    }
    
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
        public Domain.Bullet.BulletType BulletType { get; set; } 
    } 

    public class PlayerDeathEvent
    {
        public string PlayerId{get; set;}
        public Vector3 DeathPosition {get; set;}
    }
}
