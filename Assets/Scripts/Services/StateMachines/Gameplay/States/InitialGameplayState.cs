using Zenject;
using Controllers;
using System.Threading.Tasks;

namespace Services
{
    public class InitialGameplayState : IState<GameplayStates>
    {
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private InitialGameplayStateController _controller;

        public async Task Enter(ChangeStateData changeStateData)
        {
            _controller.OnStateComplete += StateCompleteHandler;
            await _controller.Init(changeStateData);
        }

        private void StateCompleteHandler()
        {
            _gameplayStateMachine.ChangeState(GameplayStates.Gameplay);
        }

        public void Exit()
        {
            _controller.OnStateComplete -= StateCompleteHandler;
        }
    }
}