using MessagePipe;
using UnityEngine;

namespace Game.Player.PlayerState
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.PlayerStateChangedEvent> publisher
        ):base(player,stateMachine,publisher){}

        public override void Enter()
        {
            PublishStateChanged("Move");
        }

        public override void Update(float deltaTime)
        {
            if(!_player.IsActive)return;
            Vector3 inputDirection = _player.GetCurrentInputDirection;
            if(inputDirection.sqrMagnitude <= 0.01f)
            {
                ChangeState<PlayerIdleState>();
            }
        }

        public override void OnDamageReceived(float damage)
        {
            ChangeState<PlayerDamageState>();
        }
    }
}
