using System;
using Entities;
using UnityEngine;
using DG.Tweening;

namespace Managers
{
    public partial class BoxManager
    {
        private Transform _parentColumn;
        
        private int _currentColumn;

        public event Action<Transform> OnColumnShifted;
        
        private void ShiftColumn(Box box, Transform parentColumn)
        {
            _parentColumn = parentColumn;
            
            _parentColumn = parentColumn;
            _currentColumn = Array.IndexOf(_columnParents, parentColumn);
            
            box.OnDespawnEvent -= ShiftColumn;

            parentColumn
                .DOMoveZ(parentColumn.position.z + 1f, 0.1f)
                .SetDelay(0.0625f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    var position = parentColumn.position;
                    position.z = Mathf.Round(position.z);
                    parentColumn.position = position;

                    OnCompleteShift();
                });
        }

        private void OnCompleteShift()
        {
            ActivateDamageReceiveFrontLineBoxes();
            AddNextBox();
            
            OnColumnShifted?.Invoke(_parentColumn);
        }

        private void ActivateDamageReceiveFrontLineBoxes()
        {
            var frontBox = _parentColumn.GetChild(0);

            frontBox
                .GetComponentInChildren<BoxHitReceiver>()
                .SetCanReceiveHit(true);
        }

        private void AddNextBox()
        {
            var dataColumn = BoxesGridConfig.Width - 1 - _currentColumn;

            var boxDataIndex = _nextBoxDataIndexPerColumn[dataColumn];

            if (boxDataIndex >= _boxDatasPerColumns[dataColumn].Count)
                return;

            var boxData = _boxDatasPerColumns[dataColumn][boxDataIndex];
            _nextBoxDataIndexPerColumn[dataColumn]++;

            var box = _poolService.Spawn<Box>(
                new Vector3(_currentColumn, 0f, -5f),
                Quaternion.identity,
                _parentColumn);

            box.Initiate(new BoxArguments
            {
                BoxData = boxData,
                ParentColumn = _parentColumn
            });

            box.OnDespawnEvent += ShiftColumn;
        }
    }
}