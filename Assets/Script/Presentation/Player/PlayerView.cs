using MessagePipe;
using R3;
using UnityEngine;
using VContainer;
using System;

namespace Presentation.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Visual Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb2d;

        [Header("Settings")]
        [SerializeField] private Game.Shared.Character.PlayerConfig playerConfig;
        public Game.Shared.Character.PlayerConfig PlayerConfig => playerConfig;
        [SerializeField] private Game.Shared.Character.CharacterStats defaultStats;
        public Game.Shared.Character.CharacterStats DefaultStats => defaultStats;

        private Game.Player.Player _player;
        public Game.Player.Player Player => _player;

        private CompositeDisposable _disposable;
        private ISubscriber<Framework.Core.Events.PlayerDamageTakenEvent> _damageTakenSubscriber;
        private ISubscriber<Framework.Core.Events.PlayerStateChangedEvent> _stateChangedSubscriber;

        private void Awake()
        {
            if(rb2d == null)rb2d = GetComponent<Rigidbody2D>();
            if(spriteRenderer == null)spriteRenderer = GetComponent<SpriteRenderer>();
            if(animator == null)animator = GetComponent<Animator>();
        }

        [Inject]
        private void Construct(ISubscriber<Framework.Core.Events.PlayerStateChangedEvent> stateChangedSubscriber,
                                ISubscriber<Framework.Core.Events.PlayerDamageTakenEvent> damageTakenSubscriber)
        {
            _stateChangedSubscriber = stateChangedSubscriber;
            _damageTakenSubscriber = damageTakenSubscriber;

            _disposable = new CompositeDisposable();
            _disposable.Add(_stateChangedSubscriber.Subscribe(OnStateChanged));
            _disposable.Add(_damageTakenSubscriber.Subscribe(OnDamageTaken));
        }

        public void InitializePlayer(Game.Player.Player player)
        {
            _player = player;
        }

        private void OnStateChanged(Framework.Core.Events.PlayerStateChangedEvent stateEvent)
        {
            string animationName = GetAnimationName(stateEvent.StateName);
            Framework.Core.CustomLogger.Log($"{stateEvent.StateName}アニメーションを受け取って再生");
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

        private void OnDamageTaken(Framework.Core.Events.PlayerDamageTakenEvent damageEvent)
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