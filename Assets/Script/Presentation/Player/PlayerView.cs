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
        [SerializeField] private string characterId = "Player_001";
        public string CharacterId => characterId;
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
            PlayAnimation(stateEvent.StateName);
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
                //animator.Play(animationName);
                Framework.Core.CustomLogger.Log("アニメーションを受け取って再生");
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