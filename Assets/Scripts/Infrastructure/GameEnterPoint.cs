using Zenject;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameEnterPoint : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;
        [Inject] private ISaveManager _saveManager;
        [Inject] private IUserManager _userManager;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            var saveData = _saveManager.Load<UserSaveData>();

            if (saveData.UserData == null) saveData.UserData = new UserData();

            _userManager.Init(saveData.UserData);
            _saveManager.Save(saveData);

            _gameManager.LoadScene(Constants.LobbyScene, LoadSceneMode.Single);
        }
    }
}