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
                var tankTransform = tanks[i].transform;

                tanks[i]
                    .GetComponent<TankMoveAttribute>()
                    .MoveTo(targetPosition)
                    .OnComplete(() =>
                    {
                        tankTransform.position = targetPosition;

                        completed++;

                        if (completed == tanks.Count)
                            OnCompleteShift();
                    });
            }
        }

        private void OnCompleteShift()
        {
            ActivateTapFrontLineTanks();
            AddNextTank();
        }

        private void ActivateTapFrontLineTanks()
        {
            var sp0 = _currentSpawnColumn.SpawnPoints[0].position;

            Tank frontTank = null;
            float bestDistance = float.MaxValue;

            foreach (Transform child in _currentSpawnColumn.ColumnParent)
            {
                if (!child.TryGetComponent(out Tank tank))
                    continue;

                var distance = Vector3.Distance(child.position, sp0);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    frontTank = tank;
                }
            }

            if (frontTank != null)
            {
                frontTank
                    .GetComponentInChildren<TankTapReceiver>()
                    .SetCanReceiveTap(true);
            }
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
        }

        public void MoveToPlacement(Tank tank)
        {
            TankPlacement placement = null;

            for (var i = 0; i < _tankPlacements.Count; i++)
            {
                if (_tankPlacements[i].IsOccupied)
                    continue;

                placement = _tankPlacements[i];
                break;
            }
            
            if (placement == null) return;
            
            placement.Occupy(tank);
            tank.GetComponent<TankPlacementAttribute>()
                .SetPlacement(placement);
            
            tank.transform.SetParent(null, true);

            tank.GetComponentInChildren<TankTapReceiver>()
                .SetCanReceiveTap(false);

            ShiftColumn(tank);

            var start = tank.transform.position;
            var end = placement.Pivot.position;

            var middle = (start + end) * 0.5f;
            middle.y += 2.0f;

            tank.transform.DOPath(
                    new[] { middle, end },
                    0.175f, PathType.CatmullRom)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    tank.transform.position = end;
                    tank.GetComponent<TankTargetProvider>().StartSearchTarget();
                });
        }
    }
}