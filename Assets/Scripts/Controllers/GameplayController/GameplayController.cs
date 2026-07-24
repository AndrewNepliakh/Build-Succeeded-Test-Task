using Zenject;
using Managers;
using Services;
using Entities;
using UnityEngine;
using System.Collections.Generic;

namespace Controllers
{
    public class GameplayController : MonoBehaviour
    {
        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private IBoxManager _boxManager;
        [Inject] private ITankManager _tankManager;
        
        
        [SerializeField] private ColumnShifter[] _columnParents = new ColumnShifter[BoxesGridConfig.Width];
        [SerializeField] private Box[] _preallocatedBoxes = new Box[BoxesGridConfig.Width * BoxesGridConfig.Height];
        [Space(20)]
        [SerializeField] private List<TanksSpawnSettings> _tanksSpawnSettings = new ();
        [SerializeField] private Tank[] _preallocatedTanks = new Tank[TanksGridConfig.Height * TanksGridConfig.MaxWidth];
        [Space(20)] 
        [SerializeField] private List<TankPlacement> _tankPlacements = new ();
        
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
            _tankManager.Initiate(_tanksSpawnSettings, _tankPlacements, _preallocatedTanks);

            _gameplayStateMachine.ChangeState(GameplayStates.Initial);
        }

        public void SetPreallocatedBoxes(Box[] preallocatedBoxes)
        {
            _preallocatedBoxes = preallocatedBoxes;
        }
        
        public void SetTanksSpawnSettings(List<TanksSpawnSettings> tanksSpawnSettings)
        {
            _tanksSpawnSettings = tanksSpawnSettings;
        }
    }
}