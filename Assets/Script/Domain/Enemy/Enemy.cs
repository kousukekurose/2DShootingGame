using UnityEngine;
using MessagePipe;

namespace Project.Domain.Enemy
{
    public class Enemy : IEnemy,Project.Domain.Character.ITargetable
    {
        private readonly string _enemyId;
        private readonly Project.Domain.Character.CharacterStats _stats;
        private readonly Project.Domain.Character.CharacterStateMachine _stateMachine;
        private readonly Project.Domain.Enemy.EnemyType _enemyType;

        private bool _isActive = true;
        private bool _isInvincible = false;
        private float _currentHP;
        private float _attackCooldownTimer = 0f;
        private Project.Domain.Character.ITargetable _currentTarget;
        private Vector3 _currentPosition;
        private bool _aiEnable = true;
        private IPublisher<Project.Application.Abstractions.Events.EnemyAttackEvent> _attackEventPublisher;

        //ICharacterの実装
        public string CharacterId => _enemyId;
        public Project.Domain.Character.CharacterStats Stats => _stats;
        public bool IsActive => _isActive;

        public bool IsValidTarget => IsActive && !IsDead;

        //IEnemyの実装
        public Project.Domain.Enemy.EnemyType EnemyType => _enemyType;
        public Project.Domain.Character.ITargetable CurrentTarget => _currentTarget;

        //IMoveの実装
        public void Move(Vector3 direction,float deltaTime)
        {
            if(!_isActive || !_aiEnable) return;
            _currentPosition += direction * _stats.MoveSpeed * deltaTime;
        }

        public Vector3 GetCurrentPosition() => _currentPosition;

        public Vector3 Position => GetCurrentPosition();

        //IAttackの実装
        public float AttackPower => _stats.AttackPower;
        public float AttackRange => _stats.AttackRange;
        public float AttackCooldown => _stats.AttackCooldown;
        public bool CanAttack => _attackCooldownTimer <= 0f && _isActive;
        public void Attack(Vector3 targetPosition, Project.Domain.Bullet.BulletType bulletType)
        {
            if(!CanAttack) return;
            _attackCooldownTimer = _stats.AttackCooldown;

            var attackEvent = new Project.Application.Abstractions.Events.EnemyAttackEvent
            {
                EnemyId = _enemyId,
                EnemyType = _enemyType,
                AttackPosition = _currentPosition,
                TargetDirection = (targetPosition - _currentPosition).normalized,
                AttackPower = _stats.AttackPower,
                Timestamp = Time.time,
                BulletType = bulletType
            };

            _attackEventPublisher?.Publish(attackEvent);
        }
        

        public void SetAttackTarget(Project.Domain.Character.ITargetable target) => _currentTarget = target;

        //IDamageの実装
        public float CurrentHP => _currentHP;
        public float MaxHP => _stats.MaxHP;
        public bool IsDead => _currentHP <= 0f;
        public bool IsInvincible => _isInvincible;

        public enum DeathReason
        {
            PlayerDamage,
            Environment,
            Timeout
        }
        private DeathReason _deathReason;
        public DeathReason DeathEvent => _deathReason;
        public void TakeDamage(float damage, Project.Domain.Character.DamageSource source)
        {
            if(IsDead || _isInvincible) return;
            float actualDamage = Mathf.Max(0f, damage - _stats.Defense);
            _currentHP -= actualDamage;
            if(_currentHP <= 0f)
            {
                _currentHP = 0f;
                _deathReason = DeathReason.PlayerDamage;
                OnDeath();
            }
        }

        public void KillByEnvironment()
        {
            _currentHP = 0f;
            _deathReason = DeathReason.Environment;
            OnDeath();
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
        public void SetTarget(Project.Domain.Character.ITargetable target) => _currentTarget = target;
        public void EnableAI() => _aiEnable = true;
        public void DisableAI() => _aiEnable = false;

        //内部メソッド
        private void OnDeath()
        {
            Deactivate();
            DisableAI();
        }

        public Project.Domain.Character.CharacterStateMachine StateMachine => _stateMachine;
        public void SetPosition(Vector3 position) => _currentPosition = position;
        public void UpdateCooldownTimer(float deltaTime)
        {
            if(_attackCooldownTimer > 0f)
            {
                _attackCooldownTimer -= deltaTime;
            }
        }

        public Enemy(string enemyId,Project.Domain.Character.CharacterStats stats,Vector3 initialPosition,Project.Domain.Enemy.EnemyType enemyType, IPublisher<Project.Application.Abstractions.Events.EnemyAttackEvent> attackEventPublisher = null)
        {
            _enemyId = enemyId;
            _stats = stats;
            _currentPosition = initialPosition;
            _enemyType = enemyType;
            _stateMachine = new Project.Domain.Character.CharacterStateMachine(this);
            _attackEventPublisher = attackEventPublisher;
            _currentHP = stats.MaxHP;
        }
    }

}

