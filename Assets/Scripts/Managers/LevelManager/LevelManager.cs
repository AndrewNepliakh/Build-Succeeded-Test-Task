using Zenject;

namespace Managers
{
    public class LevelManager : ILevelManager
    {
        [Inject] private LevelsConfig _levelsConfig;
        
        [Inject] private ISaveManager _saveManager;

        private Level _currentLevel;
        
        public Level CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }
        
        public void Init()
        {
            if (_currentLevel)
            {
                _assetsManager.ReleaseInstance(_currentLevel.gameObject);
                _currentLevel = null;
            }

            var userSaveData = _saveManager.Load<UserSaveData>();
            var levelIndex = userSaveData.UserData.LevelNumber;
            
            _levelPrefab = _levelsConfig.LevelConfigDatas[levelIndex].LevelPrefab;
        }
    }
}