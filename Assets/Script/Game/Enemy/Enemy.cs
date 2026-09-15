using UnityEngine;

namespace Game.Enemy
{
    public class Enemy : Framework.Core.Interfaces.IEnemy
    {
        public readonly string _enemyId;
        private readonly Shared.Character.CharacterStats _stats;
        private readonly Framework.Core.Patterns.CharacterStateMachine _stateMachine;
        private readonly Framework.Core.Interfaces.EnemyType _enemyType;

        private bool _isActive = true;
        private bool _isInvincible = false;
        private float _currentHP;
        private float _attackCooldownTimer = 0f;
        private Framework.Core.Interfaces.ITargetable _currentTarget;
        private Vector3 _currentPosition;
        private bool _aiEnable = true;

        //ICharacterの実装
        public string CharacterId => _enemyId;
        public Shared.Character.CharacterStats Stats => _stats;
        public bool IsActive => _isActive;

        //IEnemyの実装
        public Framework.Core.Interfaces.EnemyType EnemyType => _enemyType;
        public Framework.Core.Interfaces.ITargetable CurrentTarget => _currentTarget;

        //IMoveの実装
        public void Move(Vector3 direction,float deltaTime)
        {
            if(!_isActive || !_aiEnable) return;
            _currentPosition += direction * _stats.MoveSpeed * deltaTime;
        }

        public Vector3 GetCurrentPosition() => _currentPosition;
        public void SetMoveSpeed(float speed) =>_stats.MoveSpeed = speed;

        //IAttackの実装
        public float AttackPower => _stats.AttackPower;
        public float AttackRange => _stats.AttackRange;
        public float AttackCooldown => _attackCooldownTimer;
        public bool CanAttack => _attackCooldownTimer <= 0f && _isActive;
        public void Attack(Vector3 targetPosition)
        {
            if(!CanAttack) return;
            _attackCooldownTimer = _stats.AttackCooldown;
        }
        

        public void SetAttackTarget(Framework.Core.Interfaces.ITargetable target) => _currentTarget = target;

        //IDamageの実装
        public float CurrentHP => _currentHP;
        public float MaxHP => _stats.MaxHP;
        public bool IsDead => _currentHP <= 0f;
        public bool IsInvincible => _isInvincible;
        public void TakeDamage(float damage, Framework.Core.Interfaces.DamageSource source)
        {
            if(IsDead || _isInvincible) return;
            float actualDamage = Mathf.Max(0f, damage - _stats.Defense);
            _currentHP -= actualDamage;
            if(_currentHP <= 0f)
            {
                _currentHP = 0f;
                OnDeath();
            }
        }
        
        public void Heal(float amount)
        {
            if(IsDead)return;
            _currentHP = Mathf.Min(_currentHP + amount, _stats.MaxHP);
        }
        public void SetInvincible() => _isInvincible = true;
        public void ClearInvincible() => _isInvincible = false;

        public void Activate() => _isActive = true;
        public void Deactivate() => _isActive = false;

        //IEnemy固有メソッド
        public void SetTarget(Framework.Core.Interfaces.ITargetable target) => _currentTarget = target;
        public void EnableAI() => _aiEnable = true;
        public void DisableAI() => _aiEnable = false;

        //内部メソッド
        private void OnDeath()
        {
            Deactivate();
            DisableAI();
        }

        public Framework.Core.Patterns.CharacterStateMachine StateMachine => _stateMachine;
        public void SetPosition(Vector3 position) => _currentPosition = position;
        public void UpdateCooldownTimer(float deltaTime)
        {
            if(_attackCooldownTimer > 0f)
            {
                _attackCooldownTimer -= deltaTime;
            }
        }

        public Enemy(string enemyId,Shared.Character.CharacterStats stats,Vector3 initialPosition,Framework.Core.Interfaces.EnemyType enemyType)
        {
            _enemyId = enemyId;
            _stats = stats;
            _currentPosition = initialPosition;
            _enemyType = enemyType;
            _stateMachine = new Framework.Core.Patterns.CharacterStateMachine(this);
        }
    }

}

