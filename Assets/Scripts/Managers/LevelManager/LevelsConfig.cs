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
        [OdinSerialize] private List<LevelConfigData> _levelConfigDatas;
        public List<LevelConfigData> LevelConfigDatas => _levelConfigDatas;
    }

    [Serializable]
    public class LevelConfigData
    {
        [OdinSerialize] private int _levelDataIndex;
        public int LevelDataIndex => _levelDataIndex;

        [OdinSerialize] public LevelConfig LevelConfig = new();
    }

    public enum BoxColor
    {
        None,
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }

    [Serializable]
    public struct BoxData
    {
        public BoxColor Color;

        public int StackHeight;
    }
}