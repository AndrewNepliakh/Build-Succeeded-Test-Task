using System;
using Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class TanksGridConfig
    {
        public const int MaxWidth = 5;
        public const int Height = 3;

        public enum ColumnsCount
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        [OdinSerialize]
        [TableMatrix(
            SquareCells = true,
            ResizableColumns = false,
            DrawElementMethod = nameof(DrawGrid))]
        public TankData[,] Grid = new TankData[MaxWidth, Height];

        public void Resize(int width)
        {
            var newGrid = new TankData[width, Height];

            if (Grid != null)
            {
                var copyWidth = Math.Min(width, Grid.GetLength(0));

                for (var x = 0; x < copyWidth; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        newGrid[x, y] = Grid[x, y];
                    }
                }
            }

            Grid = newGrid;
        }
    }
}