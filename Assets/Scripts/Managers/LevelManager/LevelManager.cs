using Zenject;
using System.Collections.Generic;

namespace Managers
{
    public class LevelManager : ILevelManager
    {
        [Inject] private LevelsConfig _levelsConfig;
        [Inject] private ISaveManager _saveManager;

        public List<BoxesGridConfig> GetBoxesGridConfigsOfCurrentLevel()
        {
            var progressSaveData = _saveManager.Load<ProgressSaveData>();
            var currentLevel = progressSaveData.CurrentLevel;

            return _levelsConfig.GetBoxesGridConfigsByLevel(currentLevel);
        }
    }
}