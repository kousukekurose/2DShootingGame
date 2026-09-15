using MessagePipe;
using UnityEngine;

namespace Game.Enemy.EnemyState
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(
            Enemy enemy,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> stateChangedPublisher
        ):base(enemy ,stateMachine,stateChangedPublisher){}

        public override void Enter()
        {
            PublishStateChanged("Idle");
        }

        public override void Update(float deltaTime)
        {
            if(_enemy.CurrentTarget != null)
            {
                float distance = Vector3.Distance(_enemy.GetCurrentPosition(),_enemy.CurrentTarget.Position);
                if(distance < _enemy.Stats.AttackRange * 2f)
                {
                    ChangeState<EnemyChaseState>();
                }
            }
        }

        public override void OnDamageReceived(float damage)
        {
            ChangeState<EnemyChaseState>();
        }
    }
}
