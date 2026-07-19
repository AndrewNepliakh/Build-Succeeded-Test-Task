using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Managers
{
    [Serializable]
    public class LevelConfigData
    {
        [OdinSerialize] private List<BoxesBlockConfig> _boxesBlockConfigs = new();
        public List<BoxesBlockConfig> BoxesBlockConfigs => _boxesBlockConfigs;
    }
}