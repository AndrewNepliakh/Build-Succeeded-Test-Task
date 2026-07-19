using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class BoxesBlockConfig
    {
        [OdinSerialize][PropertyOrder(-1000)]
        private int _blockIndex;
        public int BlockIndex => _blockIndex;

        [OdinSerialize] public BoxesGridConfig boxesGridConfig = new();
    }
}