using MessagePipe;
using UnityEngine;

namespace Project.Game.Enemy.EnemyState
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

            // 縦シューティング：攻撃中も下に移動
            Vector3 downDirection = Vector3.down;
            _enemy.Move(downDirection, deltaTime);

            // 画面外に出たら削除
            if(_enemy.GetCurrentPosition().y < -10f)
            {
                _enemy.Deactivate();
                return;
            }

            Framework.Core.CustomLogger.Log($"[EnemyAttackState] CanAttack: {_enemy.CanAttack}");
            // 攻撃クールダウンがあれば攻撃
            if(_enemy.CanAttack)
            {
                // プレイヤーに向かって攻撃
                if(_enemy.CurrentTarget != null)
                {
                    Framework.Core.CustomLogger.Log("[EnemyAttackState] Attacking target");
                    _enemy.Attack(_enemy.CurrentTarget.Position, Domain.Bullet.BulletType.Normal);
                }
            }
        }
    }
}


