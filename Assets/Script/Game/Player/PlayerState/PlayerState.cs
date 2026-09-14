
using UnityEngine;
using MessagePipe;

namespace Game.Player.PlayerState
{
    public abstract class PlayerState : Framework.Core.Patterns.CharacterState
    {
        protected readonly Player _player;
        protected readonly IPublisher<Framework.Core.Events.PlayerStateChangedEvent> _stateChangedPublisher;

        protected PlayerState(
            Player player,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.PlayerStateChangedEvent> stateChangedPublisher
            )
            : base(player,stateMachine)
        {
            _player = player;
            _stateChangedPublisher = stateChangedPublisher;
        }

        protected void PublishStateChanged(string stateName)
        {
            _stateChangedPublisher.Publish(new Framework.Core.Events.PlayerStateChangedEvent 
            { 
                StateName = stateName 
            });
        }
    }
}

