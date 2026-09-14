
using MessagePipe;
using UnityEngine;

namespace Game.Player.PlayerState
{
    public class PlayerIdleState :PlayerState
    {
        public PlayerIdleState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.PlayerStateChangedEvent> publisher
        ):base(player,stateMachine,publisher){}

        public override void Enter()
        {
            PublishStateChanged("Idle");
        }

        public override void Update(float deltaTime)
        {
            if(!_player.IsActive)return;
            Vector3 inputDirection = _player.GetCurrentInputDirection;
            if(inputDirection.sqrMagnitude > 0.01f)
            {
                ChangeState<PlayerMoveState>();
            }

        }

        public override void OnDamageReceived(float damage)
        {
            ChangeState<PlayerDamageState>();
            return;
        }

    }
}
