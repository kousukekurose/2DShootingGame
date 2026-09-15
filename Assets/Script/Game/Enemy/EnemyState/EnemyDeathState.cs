using MessagePipe;

namespace Game.Enemy.EnemyState
{
    public class EnemyDeathState : EnemyState
    {
        public EnemyDeathState(
            Enemy enemy,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> stateChangedPublisher
        ):base(enemy ,stateMachine,stateChangedPublisher){}

        public override void Enter()
        {
            PublishStateChanged("Death");
            _enemy.Deactivate();
            _enemy.DisableAI();
        }
    }
}



