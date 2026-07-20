using System.Linq;
using Entities;
using Services;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;

namespace Managers
{
    public class BoxManager : IBoxManager
    {
        [Inject] private ILevelManager _levelManager;
        [Inject] private IPoolService _poolService;
        
        [Inject] private DiContainer _diContainer;

        private Transform[] _columnParents;
        private Box[] _preallocatedBoxes;
        
        public void Initiate(Transform[] columnParents, Box[] preallocatedBoxes)
        {
            _columnParents = columnParents;
            _preallocatedBoxes = preallocatedBoxes;
        }

        public void InitiatePreallocatedBoxes()
        {
            var boxesGridConfigs = _levelManager.GetBoxesGridConfigsOfCurrentLevel();
            var initialBoxGridConfig = boxesGridConfigs[0];

            var grid = initialBoxGridConfig.Grid;

            var rows = grid.GetLength(0);
            var columns = grid.GetLength(1);

            var boxesByArrayIndex = _preallocatedBoxes.ToDictionary(
                box => box.GetComponent<ArrayIndexMarker>().ArrayIndex);

            for (var x = rows - 1; x >= 0; x--)
            {
                for (var y = 0; y < columns; y++)
                {
                    var arrayIndex = new Vector2Int(y, x);

                    if (boxesByArrayIndex.TryGetValue(arrayIndex, out var box))
                    {
                        _diContainer.InjectGameObject(box.gameObject);
                        box.Initiate(grid[y, x]);
                        _poolService.Register(box);
                    }
                    
                    Debug.LogError($"Box with ArrayIndex {arrayIndex} was not found.");
                }
            }
        }

        public async Task FillInitialBoxGrid()
        {
            var boxesGridConfigs = _levelManager.GetBoxesGridConfigsOfCurrentLevel();
            var initialBoxGridConfig = boxesGridConfigs[0];

            var grid = initialBoxGridConfig.Grid;

            var rows = grid.GetLength(0);
            var columns = grid.GetLength(1);

            for (var x = rows - 1; x >= 0; x--)
            {
                for (var y = 0; y < columns; y++)
                {
                    var boxData = grid[y, x];

                    var worldX = columns - 1 - y;
                    var worldZ = x;

                    var position = new Vector3(worldX, 0, worldZ);

                    var box = await _poolService.Spawn<Box>(
                        position,
                        Quaternion.identity,
                        _columnParents[worldX]);

                    box.Initiate(boxData);
                }
            }
        }
    }
}