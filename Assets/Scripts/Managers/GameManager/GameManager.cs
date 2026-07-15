using System;
using Zenject;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : IGameManager, IInitializable, IDisposable
    {
        private GameState _gameState;
        
        public event Action<GameState> OnStateChange;
        
        private GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnStateChange?.Invoke(_gameState);
            }
        }
        
        public void Initialize()
        {
        }
        
        public void LoadScene(string sceneKey, LoadSceneMode mode)
        {
            SceneManager.LoadScene(sceneKey, mode);
        }

        public void OnPlay()
        {
            GameState = GameState.Play;
        }

        public void OnWin()
        {
            GameState = GameState.Win;
        }
        
        public void OnLose()
        {
            GameState = GameState.Lose;
        }
        
        public void Dispose()
        {
            
        }
    }
    
    public enum GameState
    {
        Initial,
        Play,
        Win,
        Lose
    }
}