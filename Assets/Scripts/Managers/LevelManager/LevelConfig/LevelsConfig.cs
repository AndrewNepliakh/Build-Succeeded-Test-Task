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
    }
}