using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Project.Infrastructure.Unity.Enemy
{
    public class EnemyInitializer
    {
        private readonly Project.Domain.Character.ITargetable _playerTraget;
        private readonly IObjectResolver _resolver;

        public EnemyInitializer(
            Project.Domain.Character.ITargetable playerTarget,
            IObjectResolver resolver
        )
        {
            _playerTraget = playerTarget;
            _resolver = resolver;
        }

        public void Initialize(Project.Domain.Enemy.Enemy enemy, Presentation.Enemy.EnemyView enemyView) 
        {
            enemyView.InitializeEnemy(enemy);
            enemyView.SetEnemyManager(_resolver.Resolve<Project.Infrastructure.Unity.Enemy.EnemyManager>());
            enemy.SetTarget(_playerTraget);
            enemy.Activate();

            var idleState = new Project.Domain.Enemy.State.EnemyIdleState(enemy,enemy.StateMachine);
            var moveState = new Project.Domain.Enemy.State.EnemyMoveState(enemy,enemy.StateMachine);
            var attackState = new Project.Domain.Enemy.State.EnemyAttackState(enemy,enemy.StateMachine);
            var deathState = new Project.Domain.Enemy.State.EnemyDeathState(enemy,enemy.StateMachine);
            
            enemy.StateMachine.RegisterState(idleState);
            enemy.StateMachine.RegisterState(moveState);
            enemy.StateMachine.RegisterState(attackState);
            enemy.StateMachine.RegisterState(deathState);

            enemy.StateMachine.ChangeState<Project.Domain.Enemy.State.EnemyIdleState>();

            var bulletFactory = _resolver.Resolve<Project.Application.Abstractions.IBulletFactory>();
            var bulletManager = _resolver.Resolve<Project.Infrastructure.Unity.Bullet.BulletManager>();
            var attackEventSubscriber = _resolver.Resolve<ISubscriber<Project.Application.Abstractions.Events.EnemyAttackEvent>>();

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


