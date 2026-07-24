using Zenject;
using Services;
using DG.Tweening;
using UnityEngine;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private TrailRenderer _trail;

        private TankShooter _tankShooter;
        private Transform _target;

        public GameObject GameObject => gameObject;
        
        public void Initialize(TankShooter tankShooter, Transform target)
        {
            transform.DOKill();

            _tankShooter = tankShooter;
            _target = target;

            transform.position = _tankShooter.FirePoint.position;

            transform
                .DOMove(target.position + Vector3.up * 0.5f, 0.15f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        _poolService.Despawn(this);
                    });
                });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_target == null)
                return;
            
            if (!other.transform.IsChildOf(_target))
                return;

            var ok = other.TryGetComponent<IHitReceiver>(out var hitReceiver);

            if (!ok)
                return;

            hitReceiver.ReceiveHit();

            _poolService.Despawn(this);
        }

        private void OnDisable()
        {
            transform.DOKill();

            _target = null;
            _tankShooter = null;
        }
        public void OnSpawn()
        {
            _trail.emitting = false;
            _trail.Clear();
            
            _trail.emitting = true;
        }

        public void OnDespawn()
        {
            _trail.emitting = false;
            _trail.Clear();
        }
    }
}
