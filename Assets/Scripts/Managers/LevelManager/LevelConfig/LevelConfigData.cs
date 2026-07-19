using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class LevelConfigData
    {
        [OdinSerialize][PropertyOrder(-1000)]
        private int _levelDataIndex;
        
        public int LevelDataIndex => _levelDataIndex;

        [OdinSerialize] public LevelConfig LevelConfig = new();
    }
}