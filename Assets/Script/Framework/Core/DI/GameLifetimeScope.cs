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
        [SerializeField] private Game.Shared.Character.EnemyConfig enemyConfig;
        [SerializeField] private Application.Enemy.EnemySystemConfig enemySystemConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe(options =>{});

            //Playerの登録
            builder.RegisterComponent(playerView);
            builder.RegisterComponent(playerInputReceiver);

            // PlayerConfigの登録
            builder.RegisterComponent(playerView.PlayerConfig);

            builder.Register<Game.Player.Player>(container =>
            {
                var view = container.Resolve<Presentation.Player.PlayerView>();
                var config = view.PlayerConfig;
                return new Game.Player.Player(
                    config.CharacterId,
                    config.DefaultStats.Clone(),
                    view.transform.position
                );
            },Lifetime.Singleton).As<Interfaces.ICharacter>().AsSelf().As<Interfaces.ITargetable>();

            builder.Register<Application.Player.PlayerMoveUseCase>(Lifetime.Singleton);
            builder.Register<Application.Player.PlayerAttackUseCase>(Lifetime.Singleton);
            builder.Register<Application.Player.PlayerDamageUseCase>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GamePlayerInitializer>();
            RegisterSystem(builder);
        }

        private void RegisterSystem(IContainerBuilder builder)
        {
            builder.RegisterComponent(enemyConfig);
            builder.RegisterComponent(enemySystemConfig);
            builder.Register<Application.Enemy.EnemyPrefabRegistry>(Lifetime.Singleton);
            builder.Register<Application.Enemy.EnemyManager>(Lifetime.Singleton);
            builder.Register<Application.Enemy.EnemyFactory>(Lifetime.Singleton);

            // 敵UseCaseの登録
            builder.Register<Application.Enemy.EnemyMoveUseCase>(Lifetime.Transient);
            builder.Register<Application.Enemy.EnemyAttackUseCase>(Lifetime.Transient);
            builder.Register<Application.Enemy.EnemyDamageUseCase>(Lifetime.Transient);

            builder.Register<Application.Enemy.EnemyInitializer>(Lifetime.Transient);

            builder.Register<Application.Enemy.EnemySpawner>(Lifetime.Singleton);
            builder.RegisterEntryPoint<EnemySystemInitializer>();
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

    public class EnemySystemInitializer : IStartable, ITickable
    {
        private readonly Application.Enemy.EnemyPrefabRegistry _prefabRegistry;
        private readonly Application.Enemy.EnemySpawner _enemySpawner;
        private readonly Application.Enemy.EnemyManager _enemyManager;
        private readonly Application.Enemy.EnemySystemConfig _config;

        public EnemySystemInitializer(
            Application.Enemy.EnemyPrefabRegistry prefabRegistry,
            Application.Enemy.EnemySpawner enemySpawner,
            Application.Enemy.EnemyManager enemyManager,
            Application.Enemy.EnemySystemConfig config)
        {
            _prefabRegistry = prefabRegistry;
            _enemySpawner = enemySpawner;
            _enemyManager = enemyManager;
            _config = config;
        }

        public void Start()
        {
            if(_config != null)
            {
                if(_config.basicEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Interfaces.EnemyType.Basic, _config.basicEnemyPrefab);
                if(_config.fastEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Interfaces.EnemyType.Fast, _config.fastEnemyPrefab);
                if(_config.tankEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Interfaces.EnemyType.Tank, _config.tankEnemyPrefab);
                if(_config.rangedEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Interfaces.EnemyType.Ranged, _config.rangedEnemyPrefab);
                if(_config.bossEnemyPrefab != null)
                    _prefabRegistry.RegisterPrefab(Interfaces.EnemyType.Boss, _config.bossEnemyPrefab);
            }

            CustomLogger.Log("Enemy system initialized with prefab registry");

            _enemySpawner.SpawnEnemyAtRandomPosition(Interfaces.EnemyType.Basic);
        }

        public void Tick()
        {
            _enemyManager.UpdateAll(Time.deltaTime);
        }
    }

}