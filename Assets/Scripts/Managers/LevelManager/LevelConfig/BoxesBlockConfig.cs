using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class BoxesBlockConfig
    {
        [OdinSerialize] private BoxesGridConfig _boxesGridConfig = new();

        public BoxesGridConfig BoxesGridConfig => _boxesGridConfig;
    }
}