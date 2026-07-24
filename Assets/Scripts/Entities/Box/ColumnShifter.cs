using System;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class ColumnShifter : MonoBehaviour
    {
        private Tween _shiftTween;

        public bool IsShifting => _shiftTween != null && _shiftTween.IsActive();

        public void Shift(Action onComplete)
        {
            if (IsShifting)
                return;

            _shiftTween = transform
                .DOMoveZ(transform.position.z + 1f, 0.075f)
                .SetDelay(0.025f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    var position = transform.position;
                    position.z = Mathf.Round(position.z);
                    transform.position = position;

                    _shiftTween = null;

                    onComplete?.Invoke();
                });
        }

        private void OnDisable()
        {
            _shiftTween?.Kill();
            _shiftTween = null;
        }
    }
}