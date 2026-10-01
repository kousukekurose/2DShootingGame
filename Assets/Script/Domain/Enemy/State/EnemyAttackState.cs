using UnityEngine;

namespace Project.Domain.Enemy.State
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(
            Enemy enemy,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(enemy ,stateMachine){}

        public override void Enter()
        {
        }

        public override void Update(float deltaTime)
        {
            if(!_enemy.IsActive) return;

            // 縦シューティング：攻撃中も下に移動
            Vector3 downDirection = Vector3.down;
            _enemy.Move(downDirection, deltaTime);

            // 画面外に出たら削除
            if(_enemy.GetCurrentPosition().y < -10f)
            {
                _enemy.Deactivate();
                return;
            }

            // 攻撃クールダウンがあれば攻撃
            if(_enemy.CanAttack)
            {
                // プレイヤーに向かって攻撃
                if(_enemy.CurrentTarget != null)
                {
                    _enemy.Attack(_enemy.CurrentTarget.Position, Project.Domain.Bullet.BulletType.Normal);
                }
            }
        }
    }
}


