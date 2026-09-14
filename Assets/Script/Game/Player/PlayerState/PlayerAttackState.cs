using MessagePipe;

namespace Game.Player.PlayerState
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.PlayerStateChangedEvent> publisher
        ):base(player,stateMachine,publisher){}

        public override void Enter()
        {
            PublishStateChanged("Attack");
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