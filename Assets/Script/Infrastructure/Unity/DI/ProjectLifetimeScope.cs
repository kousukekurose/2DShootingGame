using MessagePipe;
using Project.Application.Game;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure.Unity.DI
{
    public sealed class ProjectLifetimeScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe(options => { });
            builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);
        }
    }
}
