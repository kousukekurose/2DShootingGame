using UnityEngine;
using MessagePipe;

namespace Game.Enemy.EnemyState
{
    public class EnemyChaseState : EnemyState
    {
        public EnemyChaseState(
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
            }
            else
            {
                float distance = Vector3.Distance(_enemy.GetCurrentPosition(),_enemy.CurrentTarget.Position);

                if(distance < _enemy.Stats.AttackRange)
                {
                    //ChangeState<EnemyChaseState>();
                }
            }
        }
    }
}

