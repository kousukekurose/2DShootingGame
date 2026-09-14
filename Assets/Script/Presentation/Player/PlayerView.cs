using MessagePipe;
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

        private IDisposable _disposable;
        private IPublisher<Framework.Core.Events.PlayerStateChangedEvent> _publisher;
        private ISubscriber<Framework.Core.Events.PlayerStateChangedEvent> _subscriber;

        [Inject]
        private void Construct(ISubscriber<Framework.Core.Events.PlayerStateChangedEvent> subscribe)
        {
            _subscriber = subscribe;
            _disposable = subscribe.Subscribe(stateEvent =>
            {
                Debug.Log("アニメーション再生");
                //PlayAnimation(stateEvent.StateName);
            });
        }

        private void Awake()
        {
            if(rb2d == null)rb2d = GetComponent<Rigidbody2D>();
            if(spriteRenderer == null)spriteRenderer = GetComponent<SpriteRenderer>();
            if(animator == null)animator = GetComponent<Animator>();
        }

        public void InitializePlayer(Game.Player.Player player)
        {
            _player = player;
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
            if(animator != null) animator.Play(animationName);
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