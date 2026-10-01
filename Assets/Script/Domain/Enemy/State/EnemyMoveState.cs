using UnityEngine;

namespace Project.Domain.Enemy.State
{
    public class EnemyMoveState : EnemyState
    {
        public EnemyMoveState(
            Enemy enemy,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(enemy ,stateMachine){}

        public override void Enter()
        {
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

