using MessagePipe;

namespace Game.Enemy.EnemyState
{
    public class EnemyState : Framework.Core.Patterns.CharacterState
    {
        protected readonly Enemy _enemy;
        protected readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _stateChangedPublisher;

        protected EnemyState(
            Enemy enemy,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> stateChangedPublisher
        ):base(enemy,stateMachine)
        {
            _enemy = enemy;
            _stateChangedPublisher = stateChangedPublisher;
        }

        protected void PublishStateChanged(string stateName)
        {
            _stateChangedPublisher.Publish(new Framework.Core.Events.EnemyStateChangedEvent
            {
                StateName = stateName
            });
        }
    }
}
