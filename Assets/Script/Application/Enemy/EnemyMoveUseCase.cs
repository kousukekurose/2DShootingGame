
using System;
using R3.Triggers;
using UnityEngine;

namespace Application.Enemy
{
    public class EnemyMoveUseCase
    {
        private readonly Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;

        public EnemyMoveUseCase(Game.Enemy.Enemy enemy, Presentation.Enemy.EnemyView enemyView)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
        }
        
        public void MoveToTarget(Vector3 tragertPosition)
        {
            if(!_enemy.IsActive)return;
            Vector3 direction = (tragertPosition - _enemy.GetCurrentPosition()).normalized;
            _enemy.Move(direction,Time.deltaTime);
            _enemyView.UpdatePositionFromPhysics();
        }
    }
}