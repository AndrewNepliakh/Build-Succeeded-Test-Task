using Zenject;

namespace Managers
{
    public class LevelManager : ILevelManager
    {
        [Inject] private LevelsConfig _levelsConfig;
        [Inject] private ISaveManager _saveManager;

        public LevelConfigData GetLevelConfigOfCurrentLevel()
        {
            var progressSaveData = _saveManager.Load<ProgressSaveData>();
            var currentLevel = progressSaveData.CurrentLevel;

            return _levelsConfig.GetLevelConfigByLevel(currentLevel);
        }
    }
}