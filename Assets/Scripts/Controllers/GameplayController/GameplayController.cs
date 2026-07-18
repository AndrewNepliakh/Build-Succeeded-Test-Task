using Zenject;
using Services;
using UnityEngine;

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
            
            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }
    }
}