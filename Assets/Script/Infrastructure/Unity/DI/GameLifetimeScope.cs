using VContainer;
using VContainer.Unity;
using UnityEngine;
using MessagePipe;
using Project.Application.Abstractions.Events;

namespace Project.Infrastructure.Unity.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Scene References")]
        [SerializeField] private Presentation.Player.PlayerView playerView;
        [SerializeField] private Presentation.Player.PlayerInputReceiver playerInputReceiver;
        [SerializeField] private Project.Infrastructure.Unity.Config.EnemyConfig enemyConfig;
        [SerializeField] private Project.Infrastructure.Unity.Config.EnemySystemConfig enemySystemConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            //Playerの登録
            builder.RegisterComponent(playerView);
            builder.RegisterComponent(playerInputReceiver);

            // PlayerConfigの登録
            builder.RegisterComponent(playerView.PlayerConfig);

            builder.Register<Project.Domain.Player.Player>(container =>
            {
                var view = container.Resolve<Presentation.Player.PlayerView>();
                var config = view.PlayerConfig;
                var attackEventPublisher = container.Resolve<IPublisher<PlayerAttackEvent>>();
                var deathEventPublisher = container.Resolve<IPublisher<PlayerDeathEvent>>();
                //var damageEventPublisher = container.Resolve<IPublisher<PlayerDamageTakenEvent>>();
                return new Project.Domain.Player.Player(
                    config.CharacterId,
                    config.GetDefaultStats(),
                    view.transform.position,
                    attackEventPublisher,
                    deathEventPublisher
                );
            },Lifetime.Singleton)
                .As<Project.Domain.Character.ICharacter>()
                .AsSelf()
                .As<Project.Domain.Character.ITargetable>();

            builder.Register<Project.Infrastructure.Unity.Collision.CollisionManager>(Lifetime.Singleton);
            builder.Register<Project.Infrastructure.Unity.Config.BulletDataRegistry>(container =>
            {
                var bulletDaraArray = Resources.LoadAll<Project.Infrastructure.Unity.Config.BulletData>("Configs");
                return new Project.Infrastructure.Unity.Config.BulletDataRegistry(bulletDaraArray);
            },Lifetime.Singleton);
            builder.Register<Project.Application.Abstractions.IBulletFactory,Project.Infrastructure.Unity.Bullet.BulletFactory>(Lifetime.Singleton);
            builder.Register<Project.Infrastructure.Unity.Bullet.BulletManager>(Lifetime.Singleton)
                .AsSelf()
                .As<Project.Application.Abstractions.IBulletSpawner>();

            builder.Register<Application.Player.PlayerMoveUseCase>(Lifetime.Singleton);
            builder.Register<Application.Player.PlayerAttackUseCase>(container =>
            {
                return new Application.Player.PlayerAttackUseCase(
                    container.Resolve<Project.Domain.Player.Player>(),
                    container.Resolve<Project.Application.Abstractions.IBulletFactory>(),
                    container.Resolve<Project.Application.Abstractions.IBulletSpawner>(),
                    container.Resolve<ISubscriber<Project.Application.Abstractions.Events.PlayerAttackEvent>>()
                );
            }, Lifetime.Singleton);
            builder.Register<Application.Player.PlayerDamageUseCase>(Lifetime.Singleton);

            builder.RegisterEntryPoint<Project.Infrastructure.Unity.Runtime.PlayerRuntime>();
            RegisterSystem(builder);
        }

        private void RegisterSystem(IContainerBuilder builder)
        {
            builder.RegisterComponent(enemyConfig);
            builder.RegisterComponent(enemySystemConfig);
            builder.Register<Project.Infrastructure.Unity.Enemy.EnemyPrefabRegistry>(Lifetime.Singleton);
            builder.Register<Project.Infrastructure.Unity.Enemy.EnemyManager>(container =>
            {
                var prefabRegistry = container.Resolve<Project.Infrastructure.Unity.Enemy.EnemyPrefabRegistry>();
                var stateChangedPublisher = container.Resolve<IPublisher<Project.Presentation.Events.EnemyStateChangedEvent>>();
                return new Project.Infrastructure.Unity.Enemy.EnemyManager(prefabRegistry, stateChangedPublisher);
            },Lifetime.Singleton);
            builder.Register<Project.Infrastructure.Unity.Enemy.EnemyFactory>(Lifetime.Singleton);

            // 敵UseCaseの登録
            builder.Register<Application.Enemy.EnemyMoveUseCase>(Lifetime.Transient);
            builder.Register<Application.Enemy.EnemyDamageUseCase>(Lifetime.Transient);
            builder.Register<Project.Infrastructure.Unity.Enemy.EnemyInitializer>(Lifetime.Transient);
            builder.Register<Project.Infrastructure.Unity.Enemy.EnemySpawner>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Project.Infrastructure.Unity.Runtime.EnemyRuntime>();
        }
    }

}