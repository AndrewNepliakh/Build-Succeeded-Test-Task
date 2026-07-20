using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Managers
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
    public class LevelsConfig : SerializedScriptableObject
    {
        [OdinSerialize] private List<LevelConfigData> _levelConfigDatas = new();
        public List<LevelConfigData> LevelConfigDatas => _levelConfigDatas;

        public List<BoxesGridConfig> GetBoxesGridConfigsByLevel(int currentLevel)
        {
            if (_levelConfigDatas == null || _levelConfigDatas.Count == 0)
                return new List<BoxesGridConfig>();

            if (currentLevel < 0 || currentLevel >= _levelConfigDatas.Count)
                throw new ArgumentOutOfRangeException(nameof(currentLevel),
                    $"Level {currentLevel} does not exist. Config contains {_levelConfigDatas.Count} levels.");

            var levelConfigData = _levelConfigDatas[currentLevel];

            var result = new List<BoxesGridConfig>(levelConfigData.BoxesBlockConfigs.Count);

            foreach (var blockConfig in levelConfigData.BoxesBlockConfigs)
            {
                result.Add(blockConfig.BoxesGridConfig);
            }

            return result;
        }
    }
}