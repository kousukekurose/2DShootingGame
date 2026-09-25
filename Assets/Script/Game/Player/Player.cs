using UnityEngine;
using Framework.Core.Interfaces;
using Game.Shared.Character;
using Framework.Core.Patterns;
using MessagePipe;


namespace Game.Player
{
    public class Player : ICharacter,IMovable,IAttacker,IDamageable,ITargetable
    {
        private readonly string _characterId;
        private readonly CharacterStats _stats;
        private readonly CharacterStateMachine _stateMachine;
        public CharacterStateMachine StateMachine => _stateMachine;


        private bool _isActive = true;
        private bool _isInvincible = false;
        private float _currentHP;
        private float _attackCooldownTimer = 0f;
        private ITargetable _attackTarget;
        private Vector3 _currentPosition;
        private int _bulletIdCounter = 0;
        private IPublisher<Framework.Core.Events.PlayerAttackEvent> _attackEventPulisher;
        
        public string CharacterId => _characterId;
        public CharacterStats Stats => _stats;
        public bool IsActive => _isActive;
        public void Activate() => _isActive = true;
        private Vector3 _currentInputDirection = Vector3.zero;
        public Vector3 GetCurrentInputDirection => _currentInputDirection;

        public Vector3 Position => GetCurrentPosition();
        public bool IsValidTarget => IsActive && !IsDead;

        public void SetInputDirection(Vector3 direction)
        {
            _currentInputDirection = direction;
        }
        //IMovable実装
        public void Move(Vector3 direction,float deltaTime)
        {
            if(!_isActive)return;
            _currentPosition += direction * _stats.MoveSpeed * deltaTime;
        }

        public Vector3 GetCurrentPosition() => _currentPosition;
        public void SetMoveSpeed(float speed) => _stats.MoveSpeed = speed;

        //IAttacerの実装
        public float AttackPower => _stats.AttackPower; 
        public float AttackRange => _stats.AttackRange;
        public float AttackCooldown => _stats.AttackCooldown;
        public bool CanAttack => _attackCooldownTimer <= 0 && _isActive;

        public void Attack(Vector3 targetPosition, Domain.Bullet.BulletType bulletType = Domain.Bullet.BulletType.Normal)
        {
            if(!CanAttack)return;
            _attackCooldownTimer = _stats.AttackCooldown;

            var attackEvent = new Framework.Core.Events.PlayerAttackEvent
            {
                PlayerId = _characterId,
                AttackPosition = _currentPosition,
                TargetDirection = (targetPosition - _currentPosition).normalized,
                AttackPower = _stats.AttackPower,
                Timestamp = Time.time,
                BulletId = _bulletIdCounter++,
                BulletType = bulletType
            };

            _attackEventPulisher?.Publish(attackEvent);
        }

        public void SetAttackTarget(ITargetable target) => _attackTarget = target;

        //IDamageableの実装
        public float CurrentHP =>_currentHP;
        public float MaxHP => _stats.MaxHP;
        public bool IsDead => _currentHP <= 0f;

        public bool IsInvincible => _isInvincible;
        public void TakeDamage(float damage,DamageSource source)
        {
            if(IsDead || _isInvincible)return;
            float actalDamage = Mathf.Max(0f,damage - _stats.Defense);
            _currentHP -= actalDamage;
            if(_currentHP <= 0f)
            {
                _currentHP = 0f;
                OnDeath();
            }
        }

        public void Heal(float amout)
        {
            if(IsDead)return;
            _currentHP = Mathf.Min(_currentHP + amout,_stats.MaxHP);

        }

        public void SetInvincible() => _isInvincible = true;
        public void ClearInvincible() => _isInvincible = false;
        public void Deactivate() => _isActive = false;

        private void OnDeath()
        {
            Deactivate();
        }

        public void SetPosition(Vector3 position) => _currentPosition = position;
        public void UpdateCooldownTimer(float deltaTime)
        {
            if(_attackCooldownTimer > 0f)
            {
                _attackCooldownTimer -= deltaTime;
            }
        }
        public void SetInvincibleState(bool invicible) => _isInvincible = invicible;

        public Player(string characterId,CharacterStats stats,Vector3 initialPosition,IPublisher<Framework.Core.Events.PlayerAttackEvent> attackEventPublisher = null)
        {
            _characterId = characterId;
            _stats = stats;
            _currentPosition = initialPosition;
            _currentHP = _stats.MaxHP;
            _stateMachine = new CharacterStateMachine(this);
            _attackEventPulisher = attackEventPublisher;
        }

    }
}
