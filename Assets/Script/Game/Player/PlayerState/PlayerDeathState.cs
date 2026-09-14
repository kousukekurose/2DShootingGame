using UnityEngine;


namespace Game.Player.PlayerState
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            Application.Player.PlayerMoveUseCase  moveUseCase,
            Application.Player.PlayerAttackUseCase attackUseCase,
            Presentation.Player.PlayerView view
        ):base(player,stateMachine,moveUseCase,attackUseCase,view){}

        public override void Enter()
        {
            //_view.PlayAnimation("Death");
            //_view.SetVelocity(Vector2.zero);
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
