using Zenject;
using Managers;
using Services;
using UI;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.Bind<IUIManager>().To<UIManager>().AsSingle().NonLazy();
            Container.Bind<IBoxManager>().To<BoxManager>().AsSingle().NonLazy();
            Container.Bind<ISaveManager>().To<SaveManager>().AsSingle().NonLazy();
            Container.Bind<ILevelManager>().To<LevelManager>().AsSingle().NonLazy();
            Container.Bind<IAssetsManager>().To<AssetsManager>().AsSingle().NonLazy();

            Container.Bind(typeof(IGameManager), typeof(IInitializable)).To<GameManager>().AsSingle().NonLazy();
            
            Container.Bind<IPoolService>().To<PoolService>().AsSingle().NonLazy();
            
            Container.Bind<GameplayStateMachine<GameplayStates>>().AsSingle().NonLazy();
            
            Container.Bind<InitialGameplayState>().AsSingle().NonLazy();
            Container.Bind<WinGameplayState>().AsSingle().NonLazy();
            Container.Bind<LoseGameplayState>().AsSingle().NonLazy();
        }
    }
}