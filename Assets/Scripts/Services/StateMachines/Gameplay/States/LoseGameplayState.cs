using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services
{
    public class LoseGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Lose;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private LoseGameplayStateController _controller;
        public async Task Enter(ChangeStateData changeStateData = null)
        {
            _controller.OnStateComplete += StateCompleteHandler;
            await _controller.Init(changeStateData);
        }

        public void Exit()
        {
            _controller.OnStateComplete -= StateCompleteHandler;
        }
    
        private void StateCompleteHandler()
        {
        }
    }
}