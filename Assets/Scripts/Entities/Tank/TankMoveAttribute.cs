using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class TankMoveAttribute : MonoBehaviour, IAttribute
    {
        public void Initialize()
        {
            transform.DOKill();
        }

        public Tween MoveTo(Vector3 position)
        {
            transform.DOKill();

            return transform
                .DOMove(position, 0.1f)
                .SetEase(Ease.OutQuad);
        }
    }
}