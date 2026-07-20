using Zenject;
using Managers;
using Services;
using UnityEngine;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IBoxManager _boxManager;

        [SerializeField] private Transform[] _columnParents = new Transform[BoxesGridConfig.Width];

        [Inject]
        private void Initiate(
            InitialGameplayState initialGameplayState,
            WinGameplayState winGameplayState,
            LoseGameplayState loseGameplayState)
        {
            _gameplayStateMachine.AddState(initialGameplayState);
            _gameplayStateMachine.AddState(winGameplayState);
            _gameplayStateMachine.AddState(loseGameplayState);

            _boxManager.Initiate(_columnParents);
            
            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }
    }
}