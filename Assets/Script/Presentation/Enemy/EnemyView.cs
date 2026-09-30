using UnityEngine;
using MessagePipe;
using VContainer;
using R3;
using Cysharp.Threading.Tasks;
using System;

namespace Project.Presentation.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Visual Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb2d;

        [Header("Settings")]
        [SerializeField] private Game.Shared.Character.EnemyConfig enemyConfig; 
        public Game.Shared.Character.EnemyConfig EnemyConfig => enemyConfig;

        //[SerializeField] private Domain.Character.CharacterStats defaultStats;
        //public Domain.Character.CharacterStats DefaultStats => defaultStats;

        private Game.Enemy.Enemy _enemy;
        public Game.Enemy.Enemy Enemy => _enemy;

        private CompositeDisposable _disposable;
        private ISubscriber<Framework.Core.Events.EnemyDamageTakenEvent> _damageTakenSubscriber;
        private ISubscriber<Framework.Core.Events.EnemyStateChangedEvent> _stateChangedSubscriber;
        private Application.Enemy.EnemyAttackUseCase _attackUseCase;
        private Application.Enemy.EnemyManager _enemyManager;

        private void Awake()
        {
            if(rb2d == null) rb2d = GetComponent<Rigidbody2D>();
            if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if(animator == null) animator = GetComponent<Animator>(); 
        }

        [Inject]
        private void Construct(ISubscriber<Framework.Core.Events.EnemyDamageTakenEvent> damageTakenSubscriber,
                                ISubscriber<Framework.Core.Events.EnemyStateChangedEvent>stateChangedSubscriber)
        {
            _damageTakenSubscriber = damageTakenSubscriber;
            _stateChangedSubscriber = stateChangedSubscriber;
            _disposable = new CompositeDisposable();
            _disposable.Add(_damageTakenSubscriber.Subscribe(OnDamageTaken));
            _disposable.Add(_stateChangedSubscriber.Subscribe(OnStateChanged));
        }

        public void InitializeEnemy(Game.Enemy.Enemy enemy)
        {
            _enemy = enemy;
        }

        public void SetAttackUseCase(Application.Enemy.EnemyAttackUseCase attackUseCase)
        {
            _attackUseCase = attackUseCase;
        }

        public void SetEnemyManager(Application.Enemy.EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        private void OnStateChanged(Framework.Core.Events.EnemyStateChangedEvent stateEvent)
        {
            string animationName = GetAnimationName(stateEvent.StateName);
            Framework.Core.CustomLogger.Log("敵のアニメーションを受け取って再生");
            //PlayAnimation(animationName);
        }

        private string GetAnimationName(string stateName)
        {
            switch (stateName)
            {
                case "Idle":
                return enemyConfig.IdleAnimation;
                case "Chase":
                return enemyConfig.ChaseAnimation;
                case "Attack":
                return enemyConfig.AttackAnimation;
                case "Damage":
                return enemyConfig.DamageAnimation;
                case "Death":
                return enemyConfig.DeathAnimation;
                default:
                return enemyConfig.IdleAnimation;
            }
        }

        private void OnDamageTaken(Framework.Core.Events.EnemyDamageTakenEvent damageEvent)
        {
            SetColor(Color.red);
            ResetColorAfterDelay().Forget();
        }

        private async UniTaskVoid ResetColorAfterDelay()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                SetColor(Color.white);
                
            }
            catch(OperationCanceledException){}
        }

        public void UpdatePositionFromPhysics()
        {
            if(_enemy != null && rb2d != null)
            {
                transform.position = _enemy.GetCurrentPosition();
            }
        }

        public void SetColor(Color color)
        {
            if(spriteRenderer != null) spriteRenderer.color = color;
        }

        public void PlayAnimation(string animationName)
        {
            if(animator != null)
            {
                animator.Play(animationName);
            }
        }
        
        private void Update()
        {
            if(_enemy != null && _enemy.IsActive)
            {
                UpdatePositionFromPhysics();
            }
        }

        private void OnDestroy()
        {
            _attackUseCase?.Dispose();
            _disposable?.Dispose();
            if(_enemyManager != null && _enemy != null)
            {
                _enemyManager.ReturnEnemyViewToPool(_enemy.EnemyType,gameObject);
            }
        }
    }
}
