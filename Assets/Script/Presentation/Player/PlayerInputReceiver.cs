using UnityEngine;
using UnityEngine.InputSystem;
using R3;

namespace Presentation.Player
{
    public class PlayerInputReceiver : MonoBehaviour
    {
        private InputSystem_Actions _input;

        private readonly ReactiveProperty<Vector3> _moveDirection = new(Vector3.zero);
        private readonly Subject<Unit> _attackSubject = new();

        public ReactiveProperty<Vector3> MoveDirection => _moveDirection;
        public Subject<Unit> OnAttackRequested => _attackSubject;

        private Application.Player.PlayerMoveUseCase _moveUseCase;
        private Application.Player.PlayerAttackUseCase _attackUseCase;
        private Application.Player.PlayerDamageUseCase _damageUseCase;


        public void Initialize(Application.Player.PlayerMoveUseCase moveUseCase,Application.Player.PlayerAttackUseCase attackUseCase,Application.Player.PlayerDamageUseCase damageUseCase)
        {
            _moveUseCase = moveUseCase;
            _attackUseCase = attackUseCase;
            _damageUseCase = damageUseCase;
            Debug.Log("[PlayerInputReceiver] Initialize called - UseCases assigned");
        }

        private void OnEnable()
        {
            Debug.Log("[PlayerInputReceiver] OnEnable called - Creating InputSystem_Actions");
            _input = new InputSystem_Actions();
            _input.Enable();
            Debug.Log("[PlayerInputReceiver] InputSystem_Actions enabled");

            _input.Player.Attack.started += OnAttackStarted;
            Debug.Log("[PlayerInputReceiver] Attack event subscribed");
        }

        private void OnDisable()
        {
            if(_input != null)
            {
                _input.Player.Attack.started -= OnAttackStarted;
                _input.Disable();
            }
        }

        private void Update()
        {
            Vector2 rowInput = _input.Player.Move.ReadValue<Vector2>();
            
            if (rowInput.magnitude > 0.01f)
            {
                Debug.Log($"[PlayerInputReceiver] Move input detected: {rowInput}");
            }

            _moveDirection.Value = new Vector3(rowInput.x,rowInput.y,0f).normalized;
        }

        private void OnAttackStarted(InputAction.CallbackContext context)
        {
            _attackSubject.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            _moveDirection.Dispose();
            _attackSubject.Dispose();
        }
    }
}