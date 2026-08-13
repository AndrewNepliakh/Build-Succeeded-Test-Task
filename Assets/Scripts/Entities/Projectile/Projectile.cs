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
        
        private Tween _tween;
        
        public void Initialize(TankShooter tankShooter, Transform target)
        {
            if(_tween != null && _tween.IsActive()) return;
            
            _tankShooter = tankShooter;
            _target = target;

            transform.position = _tankShooter.FirePoint.position;

            var targetPosition = new Vector3(
                _target.position.x,
                0.5f,
                _target.position.z);

            _tween = transform.DOMove(targetPosition, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _target.GetComponent<IHitReceiver>().ReceiveHit(_tankShooter);
                _poolService.Despawn(this);
            });
        }

        private void OnDisable()
        {
            transform.DOKill();
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
