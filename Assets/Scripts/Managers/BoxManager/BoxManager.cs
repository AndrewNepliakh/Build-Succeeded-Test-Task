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
                    var boxData = grid[x, y];
                    var position = new Vector3(columns - 1 - y, 0, rows - 1 - x);
                    var box = await _poolService.Spawn<Box>(position, Quaternion.identity);
                    box.Init(boxData);
                }
            }
        }
    }
}