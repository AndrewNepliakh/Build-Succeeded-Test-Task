using System;
using Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Managers
{
    [Serializable]
    public partial class TanksGridConfig
    {
        public const int Height = 3;

        public enum ColumnsCount
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        [PropertyOrder(-100)] [LabelText("Columns")] [OdinSerialize]
        private ColumnsCount _columns = ColumnsCount.Two;

        public int Width => (int)_columns;

        [PropertyOrder(-99)]
        [Button(ButtonSizes.Large)]
        private void Create()
        {
#if UNITY_EDITOR
            if (Grid != null &&
                Grid.Length > 0 &&
                !UnityEditor.EditorUtility.DisplayDialog(
                    "Create Grid",
                    "Current grid will be lost.\n\nCreate a new grid?",
                    "Create",
                    "Cancel"))
            {
                return;
            }
#endif

            Grid = new TankData[Width, Height];
        }

        [OdinSerialize]
        [TableMatrix(
            SquareCells = true,
            ResizableColumns = false,
            DrawElementMethod = nameof(DrawGrid))]
        public TankData[,] Grid = new TankData[2, Height];
    }
}