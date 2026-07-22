using Zenject;
using Managers;
using Services;
using Entities;
using UnityEngine;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        public const int BufferPreallocatedBoxesRows = 5;
        
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IBoxManager _boxManager;


        [SerializeField] private Transform[] _columnParents = new Transform[BoxesGridConfig.Width];
        [SerializeField] private Box[] _preallocatedBoxes = new Box[BoxesGridConfig.Width * BoxesGridConfig.Height];

        [Inject]
        private void Initiate(
            InitialGameplayState initialGameplayState,
            WinGameplayState winGameplayState,
            LoseGameplayState loseGameplayState)
        {
            _gameplayStateMachine.AddState(initialGameplayState);
            _gameplayStateMachine.AddState(winGameplayState);
            _gameplayStateMachine.AddState(loseGameplayState);

            _boxManager.Initiate(_columnParents, _preallocatedBoxes);

            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }

        public void SetPreallocatedBoxes(Box[] preallocatedBoxes)
        {
            _preallocatedBoxes = preallocatedBoxes;
        }
    }
}