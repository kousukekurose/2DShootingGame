using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Project.Application.Enemy
{
    public class EnemyDamageUseCase
    {
        private readonly Project.Game.Enemy.Enemy _enemy;
        private readonly Presentation.Enemy.EnemyView _enemyView;

        public EnemyDamageUseCase(Project.Game.Enemy.Enemy enemy,Presentation.Enemy.EnemyView enemyView)
        {
            _enemy = enemy ?? throw new System.ArgumentException(nameof(enemy));
            _enemyView = enemyView ?? throw new System.ArgumentException(nameof(enemyView));
        }

        public void TakeDamage(float damage,Framework.Core.Interfaces.DamageSource source)
        {
            if(!_enemy.IsActive)return;
            _enemy.TakeDamage(damage,source);
            //_enemyView.PlayAnimation("Damage");
            _enemyView.SetColor(Color.red);
            ResetColorAfterDelay().Forget();
        }

        public void Update()
        {
            _enemy.UpdateCooldownTimer(Time.deltaTime);
        }

        private async UniTaskVoid ResetColorAfterDelay()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                _enemyView.SetColor(Color.white);
            }
            catch(OperationCanceledException){}
        }
    }
}
