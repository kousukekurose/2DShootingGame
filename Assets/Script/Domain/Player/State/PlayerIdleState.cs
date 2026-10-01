
using UnityEngine;

namespace Project.Domain.Player.State
{
    public class PlayerIdleState :PlayerState
    {
        public PlayerIdleState(
            Player player,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(player,stateMachine){}

        public override void Enter()
        {
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
        }

    }
}
