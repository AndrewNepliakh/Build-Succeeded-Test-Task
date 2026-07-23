using System;
using DG.Tweening;
using Entities;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public partial class TankManager
    {
        private TanksSpawnColumn _currentSpawnColumn;
        private int _currentColumn;

        private void ShiftColumn(Tank tank, Transform parentColumn)
        {
            tank.OnDespawnEvent -= ShiftColumn;

            for (var i = 0; i < _tanksSpawnSettings.Count; i++)
            {
                var columnIndex = _tanksSpawnSettings[i]._spawnColumns.FindIndex(
                    x => x._columnParent == parentColumn);

                if (columnIndex >= 0)
                {
                    _currentSpawnColumn = _tanksSpawnSettings[i]._spawnColumns[columnIndex];
                    _currentColumn = columnIndex;
                    break;
                }
            }

            var tanks = new List<Tank>();

            foreach (Transform child in parentColumn)
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
                var targetPosition = _currentSpawnColumn._spawnPoints[i].position;

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
            //AddNextTank();
        }
    }
}