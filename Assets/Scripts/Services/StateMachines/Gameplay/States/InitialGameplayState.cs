using System;
using Zenject;
using Managers;
using System.Threading.Tasks;
using Entities;
using UnityEngine;

namespace Services
{
    public class InitialGameplayState : IState<GameplayStates>
    {
        [Inject] private IBoxManager _boxManager;
        [Inject] private IAssetsManager _assetsManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            try
            {
                await _assetsManager.PreloadAssetAsync<Box>();
                
                _boxManager.InitiateAllBoxDatasPerColumns();
                
                _boxManager.InitiatePreallocatedBoxes();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
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