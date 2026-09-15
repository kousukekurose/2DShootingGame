using Game.Enemy.EnemyState;
using Framework.Core.Interfaces;

namespace Framework.Core.Events
{
    public class EnemyStateChangedEvent
    {
        public string StateName { get; set; }
    }

    public class EnemyDamageTakenEvent
    {
        public float Damage { get; set; }
        public DamageSource Source {get; set;}
    }
}
