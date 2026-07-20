using Zenject;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameEnterPoint : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _gameManager.LoadScene(Constants.GameScene, LoadSceneMode.Single);
        }
    }
}