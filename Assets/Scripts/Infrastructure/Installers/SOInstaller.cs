using Zenject;
using Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField] private LevelsConfig _levelsConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<LevelsConfig>().FromInstance(_levelsConfig).AsSingle().NonLazy();
    }
}