using Entities;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public partial class TankManager
    {
        private TanksSpawnColumn _currentSpawnColumn;
        private int _currentColumn;

        private void ShiftColumn(Tank tank)
        {
            for (var i = 0; i < _tanksSpawnSettings.Count; i++)
            {
                var columnIndex = _tanksSpawnSettings[i].SpawnColumns.FindIndex(
                    x => x.ColumnParent == tank.ParentColumn);

                if (columnIndex >= 0)
                {
                    _currentSpawnColumn = _tanksSpawnSettings[i].SpawnColumns[columnIndex];
                    _currentColumn = columnIndex;
                    break;
                }
            }

            var tanks = new List<Tank>();

            foreach (Transform child in tank.ParentColumn)
            {
                if (child.TryGetComponent(out Tank childTank))
                    tanks.Add(childTank);
            }

            if (tanks.Count == 0)
            {
                OnCompleteShift();
                return;
            }

            var completed = 0;

            for (var i = 0; i < tanks.Count; i++)
            {
                var targetPosition = _currentSpawnColumn.SpawnPoints[i].position;

                tanks[i]
                    .GetComponent<TankMoveAttribute>()
                    .MoveTo(targetPosition)
                    .OnComplete(() =>
                    {
                        completed++;

                        if (completed == tanks.Count)
                            OnCompleteShift();
                    });
            }
        }

        private void OnCompleteShift()
        {
            AddNextTank();
        }
        
        private void AddNextTank()
        {
            var dataColumn = _currentColumn;

            if (!_tankDatasPerColumns.TryGetValue(dataColumn, out var tankDatas))
                return;

            var tankDataIndex = _nextTankDataIndexPerColumn[dataColumn];

            if (tankDataIndex >= tankDatas.Count)
                return;

            var tankData = tankDatas[tankDataIndex];
            _nextTankDataIndexPerColumn[dataColumn]++;

            var spawnPoint = _currentSpawnColumn.SpawnPoints[^1];

            var tank = _poolService.Spawn<Tank>(
                spawnPoint.position,
                spawnPoint.rotation,
                _currentSpawnColumn.ColumnParent);

            tank.Initiate(new TankArguments
            {
                TankData = tankData,
                ParentColumn = _currentSpawnColumn.ColumnParent
            });

            var tapReceiver = tank.GetComponentInChildren<TankTapReceiver>();
            tapReceiver.SetCanReceiveTap(true);
        }

        public void MoveToPlacement(Tank tank)
        {
            ShiftColumn(tank);
        }
    }
}