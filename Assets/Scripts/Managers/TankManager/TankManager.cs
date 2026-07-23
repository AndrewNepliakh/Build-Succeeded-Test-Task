using Zenject;
using Entities;
using Services;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public partial class TankManager : ITankManager
    {
        [Inject] private ILevelManager _levelManager;
        [Inject] private IPoolService _poolService;

        [Inject] private DiContainer _diContainer;

        private List<TanksSpawnSettings> _tanksSpawnSettings;
        private Tank[] _preallocatedTanks;

        private readonly Dictionary<int, List<TankData>> _tankDatasPerColumns = new();
        private readonly Dictionary<int, int> _nextTankDataIndexPerColumn = new();

        public void Initiate(List<TanksSpawnSettings> tanksSpawnSettings, Tank[] preallocatedTanks)
        {
            _tanksSpawnSettings = tanksSpawnSettings;
            _preallocatedTanks = preallocatedTanks;
        }

        public void InitiateAllTankDatasPerColumns()
        {
            _tankDatasPerColumns.Clear();
            _nextTankDataIndexPerColumn.Clear();

            var levelConfig = _levelManager.GetLevelConfigOfCurrentLevel();
            var width = (int)levelConfig.TankColumns;

            foreach (var blockConfig in levelConfig.BoxesBlockConfigs)
            {
                var grid = blockConfig.TanksGridConfig;

                for (var column = 0; column < width; column++)
                {
                    if (!_tankDatasPerColumns.ContainsKey(column))
                    {
                        _tankDatasPerColumns[column] = new List<TankData>();
                        _nextTankDataIndexPerColumn[column] = 0;
                    }

                    for (var row = TanksGridConfig.Height - 1; row >= 0; row--)
                    {
                        _tankDatasPerColumns[column].Add(grid.Grid[column, row]);
                    }
                }
            }
        }

        public void InitiatePreallocatedTanks()
        {
            var tankIndex = 0;

            var levelConfig = _levelManager.GetLevelConfigOfCurrentLevel();
            var width = (int)levelConfig.TankColumns;

            var spawnSettings = _tanksSpawnSettings.Find(x =>
                (int)x.ColumnsCount == width);

            if (spawnSettings == null)
            {
                Debug.LogError($"TanksSpawnSettings for {width} columns was not found.");
                return;
            }

            for (var column = 0; column < width; column++)
            {
                var spawnColumn = spawnSettings._spawnColumns[column];

                for (var row = 0; row < TanksGridConfig.Height; row++)
                {
                    if (tankIndex >= _preallocatedTanks.Length)
                    {
                        Debug.LogError("Not enough preallocated tanks.");
                        return;
                    }

                    var tankDataIndex = _nextTankDataIndexPerColumn[column];

                    if (tankDataIndex >= _tankDatasPerColumns[column].Count)
                        continue;

                    var tankData = _tankDatasPerColumns[column][tankDataIndex];
                    _nextTankDataIndexPerColumn[column]++;

                    var tank = _preallocatedTanks[tankIndex++];
                    _diContainer.InjectGameObject(tank.gameObject);

                    var spawnPoint = spawnColumn._spawnPoints[TanksGridConfig.Height - 1 - row];

                    tank.transform.SetParent(spawnColumn._columnParent, false);
                    tank.transform.SetPositionAndRotation(
                        spawnPoint.position,
                        spawnPoint.rotation);

                    tank.Initiate(new TankArguments
                    {
                        TankData = tankData,
                        ParentColumn = spawnColumn._columnParent
                    });

                    tank.gameObject.SetActive(true);

                    tank.OnDespawnEvent += ShiftColumn;
                }
            }
        }
    }
}