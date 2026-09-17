using MessagePipe;
using UnityEngine;
using VContainer.Unity;

namespace Application.Enemy
{
    public class GameEnemyInitializer : MonoBehaviour, IStartable,ITickable
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;
        private readonly EnemyMoveUseCase _enemyMove;
        private readonly EnemyAttackUseCase _enemyAttack;
        private readonly EnemyDamageUseCase _enemyDamage;
        private readonly IPublisher<Framework.Core.Events.EnemyStateChangedEvent> _publisher;
        private readonly Framework.Core.Interfaces.ITargetable _playerTraget;

        public GameEnemyInitializer(
            Game.Enemy.Enemy enemy,
            Presentation.Enemy.EnemyView enemyView,
            EnemyMoveUseCase enemyMove,
            EnemyAttackUseCase enemyAttack,
            EnemyDamageUseCase enemyDamage,
            IPublisher<Framework.Core.Events.EnemyStateChangedEvent> publisher,
            Framework.Core.Interfaces.ITargetable playerTarget
        )
        {
            _enemy = enemy;
            _enemyView = enemyView;
            _enemyMove = enemyMove;
            _enemyAttack = enemyAttack;
            _enemyDamage = enemyDamage;
            _publisher = publisher;
            _playerTraget = playerTarget;
        }

        public void Start() 
        {
            _enemyView.InitializeEnemy(_enemy);

            _enemy.SetTarget(_playerTraget);

            var idleState = new Game.Enemy.EnemyState.EnemyIdleState(_enemy,_enemy.StateMachine,_publisher);
            var moveState = new Game.Enemy.EnemyState.EnemyChaseState(_enemy,_enemy.StateMachine,_publisher);
            var attackState = new Game.Enemy.EnemyState.EnemyAttackState(_enemy,_enemy.StateMachine,_publisher);
            var deathState = new Game.Enemy.EnemyState.EnemyDeathState(_enemy,_enemy.StateMachine,_publisher);
            
            _enemy.StateMachine.RegisterState(idleState);
            _enemy.StateMachine.RegisterState(moveState);
            _enemy.StateMachine.RegisterState(attackState);
            _enemy.StateMachine.RegisterState(deathState);

            _enemy.StateMachine.ChangeState<Game.Enemy.EnemyState.EnemyIdleState>();
        }

        public void Tick()
        {
            if(!_enemy.IsActive) return;
            _enemyDamage.Update();
            _enemy.StateMachine.Update(Time.deltaTime);
        }
    }
    
}


