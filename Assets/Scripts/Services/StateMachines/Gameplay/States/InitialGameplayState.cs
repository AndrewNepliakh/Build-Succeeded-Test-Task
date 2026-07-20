using Zenject;
using Managers;
using System.Threading.Tasks;

namespace Services
{
    public class InitialGameplayState : IState<GameplayStates>
    {
        [Inject] private IBoxManager _boxManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            _boxManager.InitiatePreallocatedBoxes();
            //await _boxManager.FillInitialBoxGrid();
        }

        private void StateCompleteHandler()
        {
            _gameplayStateMachine.ChangeState(GameplayStates.Gameplay);
        }

        public void Exit()
        {
            
        }
    }
}