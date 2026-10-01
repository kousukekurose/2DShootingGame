using UnityEngine;

namespace Project.Domain.Enemy.State
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(
            Enemy enemy,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(enemy ,stateMachine){}

        public override void Enter()
        {
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
