using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Application.Enemy
{
    public class EnemyInitializer
    {
        private readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _publisher;
        private readonly Framework.Core.Interfaces.ITargetable _playerTraget;
        private readonly IObjectResolver _resolver;

        public EnemyInitializer(
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> publisher,
            Framework.Core.Interfaces.ITargetable playerTarget,
            IObjectResolver resolver
        )
        {
            _publisher = publisher;
            _playerTraget = playerTarget;
            _resolver = resolver;
        }

        public void Initialize(Game.Enemy.Enemy enemy, Presentation.Enemy.EnemyView enemyView) 
        {
            enemyView.InitializeEnemy(enemy);
            enemyView.SetEnemyManager(_resolver.Resolve<Application.Enemy.EnemyManager>());
            enemy.SetTarget(_playerTraget);
            enemy.Activate();

            var idleState = new Game.Enemy.EnemyState.EnemyIdleState(enemy,enemy.StateMachine,_publisher);
            var moveState = new Game.Enemy.EnemyState.EnemyMoveState(enemy,enemy.StateMachine,_publisher);
            var attackState = new Game.Enemy.EnemyState.EnemyAttackState(enemy,enemy.StateMachine,_publisher);
            var deathState = new Game.Enemy.EnemyState.EnemyDeathState(enemy,enemy.StateMachine,_publisher);
            
            enemy.StateMachine.RegisterState(idleState);
            enemy.StateMachine.RegisterState(moveState);
            enemy.StateMachine.RegisterState(attackState);
            enemy.StateMachine.RegisterState(deathState);

            enemy.StateMachine.ChangeState<Game.Enemy.EnemyState.EnemyIdleState>();

            var bulletFactory = _resolver.Resolve<Application.Bullet.IBulletFactory>();
            var bulletManager = _resolver.Resolve<Application.Bullet.BulletManager>();
            var attackEventSubscriber = _resolver.Resolve<ISubscriber<Framework.Core.Events.EnemyAttackEvent>>();

            var attackUseCase = new Application.Enemy.EnemyAttackUseCase(
                enemy,
                enemyView,
                bulletFactory,
                bulletManager,
                attackEventSubscriber
            );

            enemyView.SetAttackUseCase(attackUseCase);
        }
    }
    
}


