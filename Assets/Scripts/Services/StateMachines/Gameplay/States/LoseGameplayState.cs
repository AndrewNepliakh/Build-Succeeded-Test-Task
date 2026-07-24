using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services
{
    public class LoseGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Lose;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
    
        public Task Enter(ChangeStateData changeStateData = null)
        {
            return Task.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}