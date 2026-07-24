using System;
using System.Linq;
using Entities;
using UnityEngine;
using DG.Tweening;

namespace Managers
{
    public partial class BoxManager
    {
        private Transform _parentColumn;
        
        private int _currentColumn;

        private Tween _shiftTween;

        public event Action<Transform> OnColumnShifted;
        
        private void ShiftColumn(Box box)
        {
            box.OnDespawnEvent -= ShiftColumn;

            var parentColumn = box.ParentColumn;
            var columnParents = _columnParents.Select(x => x.transform).ToArray();
            
            var currentColumn = Array.IndexOf(columnParents, parentColumn);
            
            var shifter = parentColumn.GetComponent<ColumnShifter>();

            shifter.Shift(() =>
            {
                ActivateDamageReceiveFrontLineBoxes(parentColumn);
                AddNextBox(parentColumn, currentColumn);

                OnColumnShifted?.Invoke(parentColumn);
            });
        }

        private void ActivateDamageReceiveFrontLineBoxes(Transform parentColumn)
        {
            foreach (Transform child in parentColumn)
            {
                if (!Mathf.Approximately(child.position.z, 9f))
                    continue;

                child
                    .GetComponentInChildren<BoxHitReceiver>()
                    .SetCanReceiveTap(true);

                break;
            }
        }

        private void AddNextBox(Transform parentColumn, int currentColumn)
        {
            var dataColumn = BoxesGridConfig.Width - 1 - currentColumn;

            var boxDataIndex = _nextBoxDataIndexPerColumn[dataColumn];

            if (boxDataIndex >= _boxDatasPerColumns[dataColumn].Count)
                return;

            var boxData = _boxDatasPerColumns[dataColumn][boxDataIndex];
            _nextBoxDataIndexPerColumn[dataColumn]++;

            var box = _poolService.Spawn<Box>(
                new Vector3(currentColumn, 0f, -5f),
                Quaternion.identity,
                parentColumn);

            box.Initiate(new BoxArguments
            {
                BoxData = boxData,
                ParentColumn = parentColumn
            });

            box.OnDespawnEvent += ShiftColumn;
        }
    }
}