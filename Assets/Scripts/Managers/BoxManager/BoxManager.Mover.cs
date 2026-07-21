using Entities;
using UnityEngine;
using DG.Tweening;

namespace Managers
{
    public partial class BoxManager
    {
        private void ShiftColumn(Box box, Transform parentColumn)
        {
            box.OnDestroy -= ShiftColumn;

            parentColumn
                .DOMoveZ(parentColumn.position.z + 1f, 0.1f)
                .SetDelay(0.0625f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => OnCompleteShift(parentColumn));
        }

        private void OnCompleteShift(Transform parentColumn)
        {
            var frontBox = parentColumn.GetChild(0);

            frontBox
                .GetComponentInChildren<BoxDamageReceiver>()
                .SetCanReceiveDamage(true);
        }
    }
}