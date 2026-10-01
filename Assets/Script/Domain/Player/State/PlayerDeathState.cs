namespace Project.Domain.Player.State
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(
            Player player,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(player,stateMachine){}

        public override void Enter()
        {
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
