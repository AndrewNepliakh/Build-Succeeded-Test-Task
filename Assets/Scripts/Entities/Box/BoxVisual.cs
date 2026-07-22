using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class BoxVisual : MonoBehaviour
    {
        [SerializeField] private float _appearDuration = 0.2f;
        [SerializeField] private Ease _appearEase = Ease.OutBack;

        private Tween _scaleTween;

        private void OnEnable()
        {
            PlayAppearAnimation();
        }
        
        private void PlayAppearAnimation()
        {
            _scaleTween?.Kill();

            transform.localScale = Vector3.zero;

            _scaleTween = transform
                .DOScale(Vector3.one, _appearDuration)
                .SetEase(_appearEase);
        }

        private void OnDisable()
        {
            _scaleTween?.Kill();
            transform.localScale = Vector3.zero;
        }
    }
}