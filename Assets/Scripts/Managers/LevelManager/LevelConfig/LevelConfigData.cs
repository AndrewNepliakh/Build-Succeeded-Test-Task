using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Managers
{
    [Serializable]
    public class LevelConfigData
    {
        [OdinSerialize][PropertyOrder(-1000)]
        private int _levelIndex;
        public int LevelIndex => _levelIndex;
        
        [OdinSerialize] private List<BoxesBlockConfig> _boxesBlockConfigs = new();
        public List<BoxesBlockConfig> BoxesBlockConfigs => _boxesBlockConfigs;
    }
}