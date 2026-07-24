using System;
using Zenject;
using Managers;
using Entities;
using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public class InitialGameplayState : IState<GameplayStates>
    {
        [Inject] private IBoxManager _boxManager;
        [Inject] private ITankManager _tankManager;
        [Inject] private IAssetsManager _assetsManager;
        
        public GameplayStates State => GameplayStates.Initial;

        [Inject] private GameplayStateMachine<GameplayStates> _gameplayStateMachine;

        public async Task Enter(ChangeStateData changeStateData)
        {
            try
            {
                _boxManager.InitiateAllBoxDatasPerColumns();
                _tankManager.InitiateAllTankDatasPerColumns();
                
                _boxManager.InitiatePreallocatedBoxes();
                _tankManager.InitiatePreallocatedTanks();
                
                await _assetsManager.PreloadAssetAsync<Box>();
                
                _boxManager.CreateBufferBoxes();
                
                await _assetsManager.PreloadAssetAsync<Tank>();
                await _assetsManager.PreloadAssetAsync<BoxVisual>();
                await _assetsManager.PreloadAssetAsync<Projectile>();
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