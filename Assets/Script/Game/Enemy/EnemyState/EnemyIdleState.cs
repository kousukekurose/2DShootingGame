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
            // 縦シューティング：即座に攻撃状態に遷移
            ChangeState<EnemyAttackState>();
        }

        public override void OnDamageReceived(float damage)
        {
            ChangeState<EnemyMoveState>();
        }
    }
}
