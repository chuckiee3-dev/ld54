using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Tower tower; 
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<Tower>(tower);
        builder.Register<IInputEvents, InputEvents>(Lifetime.Singleton);
        builder.Register<IWordEvents, WordEvents>(Lifetime.Singleton);
        builder.Register<IEnemyEvents, EnemyEvents>(Lifetime.Singleton);
        builder.Register<IGameEvents, GameEvents>(Lifetime.Singleton);
        builder.Register<IBaseEvents, BaseEvents>(Lifetime.Singleton);
        builder.Register<IUpgradeProvider, UpgradeProvider>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InputManager>();
        builder.RegisterEntryPoint<WordTracker>().AsSelf();
        builder.Register<AddressableDictionaryProvider>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
