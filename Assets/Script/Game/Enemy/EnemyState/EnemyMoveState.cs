using UnityEngine;
using MessagePipe;

namespace Game.Enemy.EnemyState
{
    public class EnemyMoveState : EnemyState
    {
        public EnemyMoveState(
            Enemy enemy,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> stateChangedPublisher
        ):base(enemy ,stateMachine,stateChangedPublisher){}

        public override void Enter()
        {
            PublishStateChanged("Chase");
        }

        public override void Update(float deltaTime)
        {
            if(_enemy.CurrentTarget == null)
            {
                ChangeState<EnemyIdleState>();
                return;
            }
            
            Vector3 downDirection = Vector3.down;
            _enemy.Move(downDirection,deltaTime);

            if(_enemy.GetCurrentPosition().y < -10f)
            {
                _enemy.Deactivate();
            }
        }
    }
}

