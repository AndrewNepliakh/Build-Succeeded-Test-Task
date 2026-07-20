using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services
{
    public class LoseGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Lose;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
    
        public async Task Enter(ChangeStateData changeStateData = null)
        {
        }

        public void Exit()
        {
        }
    }
}