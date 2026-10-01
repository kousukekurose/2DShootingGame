using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

namespace Project.Presentation.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Visual Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb2d;

        [Header("Settings")]
        [SerializeField] private Project.Infrastructure.Unity.Config.PlayerConfig playerConfig;
        public Project.Infrastructure.Unity.Config.PlayerConfig PlayerConfig => playerConfig;
        //[SerializeField] private Project.Domain.Character.CharacterStats defaultStats;
        //public Project.Domain.Character.CharacterStats DefaultStats => defaultStats;

        private Project.Domain.Player.Player _player;
        public Project.Domain.Player.Player Player => _player;

        private CompositeDisposable _disposable;
        private ISubscriber<Project.Application.Abstractions.Events.PlayerDamageTakenEvent> _damageTakenSubscriber;
        private ISubscriber<Project.Presentation.Events.PlayerStateChangedEvent> _stateChangedSubscriber;

        private void Awake()
        {
            if(rb2d == null)rb2d = GetComponent<Rigidbody2D>();
            if(spriteRenderer == null)spriteRenderer = GetComponent<SpriteRenderer>();
            if(animator == null)animator = GetComponent<Animator>();
        }

        [Inject]
        private void Construct(ISubscriber<Project.Presentation.Events.PlayerStateChangedEvent> stateChangedSubscriber,
                                ISubscriber<Project.Application.Abstractions.Events.PlayerDamageTakenEvent> damageTakenSubscriber)
        {
            _stateChangedSubscriber = stateChangedSubscriber;
            _damageTakenSubscriber = damageTakenSubscriber;

            _disposable = new CompositeDisposable();
            _disposable.Add(_stateChangedSubscriber.Subscribe(OnStateChanged));
            _disposable.Add(_damageTakenSubscriber.Subscribe(OnDamageTaken));
        }

        public void InitializePlayer(Project.Domain.Player.Player player)
        {
            _player = player;
        }

        private void OnStateChanged(Project.Presentation.Events.PlayerStateChangedEvent stateEvent)
        {
            string animationName = GetAnimationName(stateEvent.StateName);
            Project.Infrastructure.Unity.Logging.UnityLogger.Log($"{stateEvent.StateName}アニメーションを受け取って再生");
            //PlayAnimation(animationName);
        }

        private string GetAnimationName(string stateName)
        {
            switch (stateName)
            {
                case "Idle":
                return playerConfig.IdleAnimation;
                case "Move":
                return playerConfig.MoveAnimation;
                case "Attack":
                return playerConfig.AttackAnimation;
                case "Damage":
                return playerConfig.DamageAnimation;
                case "Death":
                return playerConfig.DeathAnimation;
                default:
                return playerConfig.IdleAnimation;
            }
        }

        private void OnDamageTaken(Project.Application.Abstractions.Events.PlayerDamageTakenEvent damageEvent)
        {
            SetColor(Color.red);
        }

        private void FixedUpdate()
        {
            if(_player != null && rb2d != null)
            {
                _player.SetPosition(transform.position);
            }
            
        }

        public void UpdatePositionFromPhysics()
        {
            if(_player != null && rb2d != null)
            {
                transform.position = _player.GetCurrentPosition();
            }
        }

        public void PlayAnimation(string animationName)
        {
            if(animator != null) 
            {
                animator.Play(animationName);
            }
        }

        public void SetVisible(bool visible)
        {
            if(spriteRenderer != null) spriteRenderer.enabled = visible;
        }

        public void SetColor(Color color)
        {
            if(spriteRenderer != null) spriteRenderer.color = color;
        }

        private void OnDestroy() => _player = null;

    }
}