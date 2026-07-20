using Zenject;
using Managers;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.Bind<ISaveManager>().To<SaveManager>().AsSingle().NonLazy();

            Container.Bind(typeof(IGameManager), typeof(IInitializable)).To<GameManager>().AsSingle().NonLazy();
        }
    }
}