using MessagePipe;
using UnityEngine;

namespace Game.Enemy.EnemyState
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(
            Enemy enemy,
            Framework.Core.Patterns.CharacterStateMachine stateMachine,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> stateChangedPublisher
        ):base(enemy ,stateMachine,stateChangedPublisher){}

        public override void Enter()
        {
            PublishStateChanged("Attack");
        }

        public override void Update(float deltaTime)
        {
            if(!_enemy.IsActive) return;

            if(_enemy.CurrentTarget == null)
            {
                ChangeState<EnemyIdleState>();
                return;
            }

            float distance = Vector3.Distance(_enemy.GetCurrentPosition(),_enemy.CurrentTarget.Position);
            if(distance > _enemy.Stats.AttackRange)
            {
                ChangeState<EnemyChaseState>();
            }
            
            if(_enemy.CanAttack)
            {
                if(_enemy.CurrentTarget != null)
                {
                    _enemy.Attack(_enemy.CurrentTarget.Position, Domain.Bullet.BulletType.Normal);
                }
            }
        }
    }
}


