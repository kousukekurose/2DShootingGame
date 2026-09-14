using UnityEngine;

namespace Framework.Core.Events
{
    public class PlayerStateChangedEvent
    {
        public string StateName { get; set; }
    }
    
    public class PlayerDamageTakenEvent
    {
        public float Damage { get; set; }
    }
}
