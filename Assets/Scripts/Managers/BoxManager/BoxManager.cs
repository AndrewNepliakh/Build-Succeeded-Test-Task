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

        private Transform[] _columnParents;
        
        public void Initiate(Transform[] columnParents)
        {
            _columnParents = columnParents;
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