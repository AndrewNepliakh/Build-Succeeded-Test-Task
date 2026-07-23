using System;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Managers
{
    [Serializable]
    public class LevelConfigData
    {
        [OdinSerialize]
        private TanksGridConfig.ColumnsCount _tankColumns =
            TanksGridConfig.ColumnsCount.Three;

        public TanksGridConfig.ColumnsCount TankColumns => _tankColumns;

        [OdinSerialize]
        private List<BoxesBlockConfig> _boxesBlockConfigs = new();

        public List<BoxesBlockConfig> BoxesBlockConfigs => _boxesBlockConfigs;
    }
}