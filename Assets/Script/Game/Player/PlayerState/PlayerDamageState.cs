using MessagePipe;

namespace Game.Player.PlayerState
{
    public class PlayerDamageState : PlayerState
    {
        public PlayerDamageState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.PlayerStateChangedEvent> publisher
        ):base(player,stateMachine,publisher){}

        public override void Enter()
        {
            PublishStateChanged("Damage");
        }

        public override void Update(float deltaTime)
        {
            if(!_player.IsActive)return;
        }

        public override void OnDamageReceived(float damage)
        {
            
        }
    }
}
