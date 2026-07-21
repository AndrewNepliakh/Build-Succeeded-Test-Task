using Zenject;
using Entities;
using Services;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Managers
{
    public partial class BoxManager : IBoxManager
    {
        [Inject] private ILevelManager _levelManager;
        [Inject] private IPoolService _poolService;

        [Inject] private DiContainer _diContainer;

        private Transform[] _columnParents;
        private Box[] _preallocatedBoxes;

        private readonly Dictionary<int, List<BoxData>> _boxDatasPerColumns = new ();
        private readonly Dictionary<int, int> _nextBoxDataIndexPerColumn = new();

        public void Initiate(Transform[] columnParents, Box[] preallocatedBoxes)
        {
            _columnParents = columnParents;
            _preallocatedBoxes = preallocatedBoxes;
        }

        public void InitiateAllBoxDatasPerColumns()
        {
            _boxDatasPerColumns.Clear();
            _nextBoxDataIndexPerColumn.Clear();

            var boxesGridConfigs = _levelManager.GetBoxesGridConfigsOfCurrentLevel();

            for (var column = 0; column < BoxesGridConfig.Width; column++)
            {
                _boxDatasPerColumns[column] = new List<BoxData>();
                _nextBoxDataIndexPerColumn[column] = 0;
            }

            foreach (var boxesGridConfig in boxesGridConfigs)
            {
                for (var column = 0; column < BoxesGridConfig.Width; column++)
                {
                    for (var row = BoxesGridConfig.Height - 1; row >= 0; row--)
                    {
                        _boxDatasPerColumns[column].Add(boxesGridConfig.Grid[column, row]);
                    }
                }
            }
        }

        public void InitiatePreallocatedBoxes()
        {
            var rows = BoxesGridConfig.Height;
            var columns = BoxesGridConfig.Width;

            var boxesByArrayIndex = _preallocatedBoxes.ToDictionary(
                box => box.GetComponent<ArrayIndexMarker>().ArrayIndex);

            for (var row = rows - 1; row >= 0; row--)
            {
                for (var column = 0; column < columns; column++)
                {
                    var arrayIndex = new Vector2Int(column, row);

                    if (boxesByArrayIndex.TryGetValue(arrayIndex, out var box))
                    {
                        _diContainer.InjectGameObject(box.gameObject);

                        var boxDataIndex = _nextBoxDataIndexPerColumn[column];
                        
                        var worldX = columns - 1 - column;

                        var args = new BoxArguments
                        {
                            BoxData = _boxDatasPerColumns[column][boxDataIndex],
                            ParentColumn = _columnParents[worldX]
                        };

                        box.Initiate(args);
                        
                        var damageReceiver = box.GetComponentInChildren<BoxDamageReceiver>();
                        damageReceiver.SetCanReceiveDamage(row == rows - 1);
                        
                        box.OnDestroy += ShiftColumn;

                        _nextBoxDataIndexPerColumn[column]++;

                        _poolService.Register(box);
                    }
                    else
                    {
                        Debug.LogError($"Box with ArrayIndex {arrayIndex} was not found.");
                    }
                }
            }
        }

        public async Task CreateBufferBoxes()
        {
            const int bufferRows = 5;
            var columns = BoxesGridConfig.Width;

            for (var row = 0; row < bufferRows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var boxDataIndex = _nextBoxDataIndexPerColumn[column];

                    if (boxDataIndex >= _boxDatasPerColumns[column].Count)
                        continue;

                    var boxData = _boxDatasPerColumns[column][boxDataIndex];
                    _nextBoxDataIndexPerColumn[column]++;

                    var worldX = columns - 1 - column;
                    var worldZ = -(row + 1);

                    var position = new Vector3(worldX, 0, worldZ);

                    var box = await _poolService.Spawn<Box>(
                        position,
                        Quaternion.identity,
                        _columnParents[worldX]);
                    
                    var args = new BoxArguments
                    {
                        BoxData = boxData,
                        ParentColumn = _columnParents[worldX]
                    };

                    box.Initiate(args);
                    
                    box.OnDestroy += ShiftColumn;
                }
            }
        }
    }
}