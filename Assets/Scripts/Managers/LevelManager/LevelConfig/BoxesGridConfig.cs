using System;
using Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class BoxesGridConfig
    {
        public const int Width = 10;
        public const int Height = 10;

        [OdinSerialize]
        [TableMatrix(SquareCells = true, ResizableColumns = false, DrawElementMethod = nameof(DrawGrid))]
        public BoxData[,] Grid = new BoxData[Width, Height];
    }
}
