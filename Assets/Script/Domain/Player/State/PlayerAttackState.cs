namespace Project.Domain.Player.State
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(
            Player player,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(player,stateMachine){}

        public override void Enter()
        {
        }

        public override void Update(float deltaTime)
        {
            if(!_player.IsActive)return;

            _player.UpdateCooldownTimer(deltaTime);

            if(_player.CanAttack)
            {
                _stateMachine.ChangeState<PlayerIdleState>();
            }
        }

        public override void OnDamageReceived(float damage)
        {
            
        }
    }
}