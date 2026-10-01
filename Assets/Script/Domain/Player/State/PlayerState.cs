
namespace Project.Domain.Player.State
{
    public abstract class PlayerState : Project.Domain.Character.CharacterState
    {
        protected readonly Player _player;

        protected PlayerState(
            Player player,
            Project.Domain.Character.CharacterStateMachine stateMachine
            )
            : base(player,stateMachine)
        {
            _player = player;
        }
    }
}

