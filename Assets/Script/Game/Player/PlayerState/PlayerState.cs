
using UnityEngine;

namespace Game.Player.PlayerState
{
    public abstract class PlayerState : Framework.Core.Patterns.CharacterState
    {
        protected readonly Player _player;
        protected readonly Application.Player.PlayerMoveUseCase _moveUseCase;
        protected readonly Application.Player.PlayerAttackUseCase _attackUseCase;
        protected readonly new Presentation.Player.PlayerView _view;

        protected PlayerState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            Application.Player.PlayerMoveUseCase moveUseCase,
            Application.Player.PlayerAttackUseCase attackUseCase,
            Presentation.Player.PlayerView view
        ): base(player,stateMachine,view)
        {
            _player = player;
            _moveUseCase = moveUseCase;
            _attackUseCase = attackUseCase;
            _view = view;
        }
    }
}

