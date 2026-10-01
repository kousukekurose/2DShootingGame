using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace Project.Infrastructure.Unity.Runtime
{
    public sealed class PlayerRuntime : IStartable, ITickable
    {
        private readonly Project.Domain.Player.Player _player;
        private readonly Project.Presentation.Player.PlayerView _playerView;
        private readonly Project.Application.Player.PlayerMoveUseCase _moveUseCase;
        private readonly Project.Application.Player.PlayerAttackUseCase _attackUseCase;
        private readonly Project.Application.Player.PlayerDamageUseCase _damageUseCase;
        private readonly Project.Presentation.Player.PlayerInputReceiver _inputReceiver;
        private readonly IPublisher<Project.Presentation.Events.PlayerStateChangedEvent> _publisher;
        private string _lastStateName;

        public PlayerRuntime(
            Project.Domain.Player.Player player,
            Project.Presentation.Player.PlayerView playerView,
            Project.Application.Player.PlayerMoveUseCase moveUseCase,
            Project.Application.Player.PlayerAttackUseCase attackUseCase,
            Project.Application.Player.PlayerDamageUseCase damageUseCase,
            Project.Presentation.Player.PlayerInputReceiver inputReceiver,
            IPublisher<Project.Presentation.Events.PlayerStateChangedEvent> publisher)
        {
            _player = player;
            _playerView = playerView;
            _moveUseCase = moveUseCase;
            _attackUseCase = attackUseCase;
            _damageUseCase = damageUseCase;
            _inputReceiver = inputReceiver;
            _publisher = publisher;
        }

        public void Start()
        {
            _playerView.InitializePlayer(_player);
            _inputReceiver.Initialize(_moveUseCase, _attackUseCase, _damageUseCase);

            _player.StateMachine.RegisterState(new Project.Domain.Player.State.PlayerIdleState(_player, _player.StateMachine));
            _player.StateMachine.RegisterState(new Project.Domain.Player.State.PlayerMoveState(_player, _player.StateMachine));
            _player.StateMachine.RegisterState(new Project.Domain.Player.State.PlayerAttackState(_player, _player.StateMachine));
            _player.StateMachine.RegisterState(new Project.Domain.Player.State.PlayerDamageState(_player, _player.StateMachine));
            _player.StateMachine.RegisterState(new Project.Domain.Player.State.PlayerDeathState(_player, _player.StateMachine));
            _player.StateMachine.ChangeState<Project.Domain.Player.State.PlayerIdleState>();
        }

        public void Tick()
        {
            _damageUseCase.Update();
            _player.SetPosition(_playerView.transform.position);
            _player.StateMachine.Update(Time.deltaTime);
            PublishStateChange();
        }

        private void PublishStateChange()
        {
            var currentState = _player.StateMachine.CurrentState;
            if (currentState == null) return;

            var stateName = currentState.GetType().Name
                .Replace("Player", string.Empty)
                .Replace("State", string.Empty);

            if (stateName == _lastStateName) return;

            _lastStateName = stateName;
            _publisher.Publish(new Project.Presentation.Events.PlayerStateChangedEvent
            {
                StateName = stateName,
                Timestamp = Time.time
            });
        }
    }
}
