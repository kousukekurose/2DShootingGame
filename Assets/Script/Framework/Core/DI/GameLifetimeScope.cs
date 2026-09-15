using VContainer;
using VContainer.Unity;
using UnityEngine;
using MessagePipe;

namespace Framework.Core.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Scene References")]
        [SerializeField] private Presentation.Player.PlayerView playerView;
        [SerializeField] private Presentation.Player.PlayerInputReceiver playerInputReceiver;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe(options =>{});
            builder.RegisterComponent(playerView);
            builder.RegisterComponent(playerInputReceiver);

            builder.Register<Game.Player.Player>(container =>
            {
                var view = container.Resolve<Presentation.Player.PlayerView>();
                return new Game.Player.Player(
                    view.CharacterId,
                    view.DefaultStats.Clone(),
                    view.transform.position
                );
            },Lifetime.Singleton).As<Framework.Core.Interfaces.ICharacter>().AsSelf();

            builder.Register<Application.Player.PlayerMoveUseCase>(Lifetime.Singleton);
            builder.Register<Application.Player.PlayerAttackUseCase>(Lifetime.Singleton);
            builder.Register<Application.Player.PlayerDamageUseCase>(Lifetime.Singleton);


            builder.RegisterEntryPoint<GamePlayerInitializer>();
        }
    }

    public class GamePlayerInitializer : IStartable,ITickable
    {
        private readonly Game.Player.Player _player;
        private readonly Presentation.Player.PlayerView _playerView;
        private readonly Application.Player.PlayerMoveUseCase _moveUseCase;
        private readonly Application.Player.PlayerAttackUseCase _attackUseCase;
        private readonly Application.Player.PlayerDamageUseCase _damageUseCase;
        private readonly Presentation.Player.PlayerInputReceiver _inputReceiver;
        private readonly IPublisher<Events.PlayerStateChangedEvent> _publisher;
        public GamePlayerInitializer(
            Game.Player.Player player,
            Presentation.Player.PlayerView playerView,
            Application.Player.PlayerMoveUseCase moveUseCase,
            Application.Player.PlayerAttackUseCase attackUseCase,
            Application.Player.PlayerDamageUseCase damageUseCase,
            Presentation.Player.PlayerInputReceiver inputReceiver,
            IPublisher<Events.PlayerStateChangedEvent> publisher
        )
        {
            _player = player;
            _playerView = playerView;
            _moveUseCase = moveUseCase;
            _attackUseCase = attackUseCase;
            _damageUseCase = damageUseCase;
            _inputReceiver = inputReceiver;
            _publisher = publisher;
        }

        public void Start()
        {
            CustomLogger.Log("[GamePlayerInitializer] Start called");
            _playerView.InitializePlayer(_player);
            
            CustomLogger.Log("[GamePlayerInitializer] Initializing PlayerInputReceiver");
            _inputReceiver.Initialize(_moveUseCase, _attackUseCase, _damageUseCase);
            
            CustomLogger.Log("[GamePlayerInitializer] Creating states...");
            var idleState = new Game.Player.PlayerState.PlayerIdleState(_player, _player.StateMachine,_publisher);
            var moveState = new Game.Player.PlayerState.PlayerMoveState(_player, _player.StateMachine,_publisher);
            var attackState = new Game.Player.PlayerState.PlayerAttackState(_player, _player.StateMachine,_publisher);
            var damageState = new Game.Player.PlayerState.PlayerDamageState(_player, _player.StateMachine,_publisher);
            var deathState = new Game.Player.PlayerState.PlayerDeathState(_player, _player.StateMachine,_publisher);
            
            CustomLogger.Log("[GamePlayerInitializer] Registering states...");
            _player.StateMachine.RegisterState(idleState);
            _player.StateMachine.RegisterState(moveState);
            _player.StateMachine.RegisterState(attackState);
            _player.StateMachine.RegisterState(damageState);
            _player.StateMachine.RegisterState(deathState);
            
            CustomLogger.Log("[GamePlayerInitializer] Changing to IdleState");
            _player.StateMachine.ChangeState<Game.Player.PlayerState.PlayerIdleState>();
        }

        public void Tick()
        {   
            _damageUseCase.Update();
            _player.StateMachine.Update(Time.deltaTime);
        }
    }
}