using Zenject;
using Services;
using UnityEngine;
using System.Threading.Tasks;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        [Inject]
        private void Instantiation(
            WinGameplayState winGameplayState,
            LoseGameplayState loseGameplayState)
        {
            _gameplayStateMachine.AddState(winGameplayState);
            _gameplayStateMachine.AddState(loseGameplayState);
        }

        public async void StartGameplay()
        {
            while (_gameplayStateMachine == null)
            {
                await Task.Yield();
            }
            
            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }
    }
}