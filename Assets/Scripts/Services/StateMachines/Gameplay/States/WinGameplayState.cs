using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services
{
    public class WinGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Win;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        
        public async Task Enter(ChangeStateData changeStateData = null)
        {
        }

        public void Exit()
        {
        }
    }
}