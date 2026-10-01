using UnityEngine;

namespace Project.Application.Enemy
{
    public class EnemyAIUseCase
    {
        private readonly Project.Domain.Enemy.Enemy _enemy;
        private readonly EnemyMoveUseCase _moveUseCase;
        private readonly EnemyAttackUseCase _attackUseCase;
        private readonly Project.Domain.Character.ITargetable _playerTarget;

        public EnemyAIUseCase(
        Project.Domain.Enemy.Enemy enemy,
        EnemyMoveUseCase moveUseCase,
        EnemyAttackUseCase attackUseCase,
        Project.Domain.Character.ITargetable playerTarget)
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
