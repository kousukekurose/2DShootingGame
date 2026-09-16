using UnityEngine;

namespace Application.Enemy
{
    public class EnemyAIUseCase
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly EnemyMoveUseCase _moveUseCase;
        private readonly EnemyAttackUseCase _attackUseCase;
        private readonly Framework.Core.Interfaces.ITargetable _playerTarget;

        public EnemyAIUseCase(
        Game.Enemy.Enemy enemy,
        EnemyMoveUseCase moveUseCase,
        EnemyAttackUseCase attackUseCase,
        Framework.Core.Interfaces.ITargetable playerTarget)
        {
            _enemy = enemy;
            _moveUseCase = moveUseCase;
            _attackUseCase = attackUseCase;
            _playerTarget = playerTarget;
        }

        public void Initialize()
        {
        _enemy.SetTarget(_playerTarget);
        }

        public void Update()
        {
            if(!_enemy.IsActive) return;
        
            // ステートマシンを更新
            _enemy.StateMachine.Update(Time.deltaTime);
        }
    }
}
