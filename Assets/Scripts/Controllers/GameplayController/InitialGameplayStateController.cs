using UI;
using System;
using Zenject;
using Managers;
using Services;
using UnityEngine;
using System.Threading.Tasks;

namespace Controllers
{
    public class InitialGameplayStateController : IGameplayStateController
    {
        [Inject] private IUIManager _uiManager;
        [Inject] private IGameManager _gameManager;
        [Inject] private ILevelManager _levelManager;
        
        public Action OnStateComplete;
        
        public async Task Init(ChangeStateData changeStateData)
        {
            try
            {
                if(_levelManager.CurrentLevel == null) InitLevel();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private void InitLevel()
        {
            _levelManager.Init();
        }
    }
}