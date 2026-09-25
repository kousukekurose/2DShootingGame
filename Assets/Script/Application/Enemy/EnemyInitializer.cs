using MessagePipe;
using UnityEngine;

namespace Application.Enemy
{
    public class EnemyInitializer
    {
        private readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _publisher;
        private readonly Framework.Core.Interfaces.ITargetable _playerTraget;

        public EnemyInitializer(
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> publisher,
            Framework.Core.Interfaces.ITargetable playerTarget
        )
        {
            _publisher = publisher;
            _playerTraget = playerTarget;
        }

        public void Initialize(Game.Enemy.Enemy enemy, Presentation.Enemy.EnemyView enemyView) 
        {
            enemyView.InitializeEnemy(enemy);
            enemy.SetTarget(_playerTraget);

            var idleState = new Game.Enemy.EnemyState.EnemyIdleState(enemy,enemy.StateMachine,_publisher);
            var moveState = new Game.Enemy.EnemyState.EnemyChaseState(enemy,enemy.StateMachine,_publisher);
            var attackState = new Game.Enemy.EnemyState.EnemyAttackState(enemy,enemy.StateMachine,_publisher);
            var deathState = new Game.Enemy.EnemyState.EnemyDeathState(enemy,enemy.StateMachine,_publisher);
            
            enemy.StateMachine.RegisterState(idleState);
            enemy.StateMachine.RegisterState(moveState);
            enemy.StateMachine.RegisterState(attackState);
            enemy.StateMachine.RegisterState(deathState);

            enemy.StateMachine.ChangeState<Game.Enemy.EnemyState.EnemyIdleState>();
        }
    }
    
}


