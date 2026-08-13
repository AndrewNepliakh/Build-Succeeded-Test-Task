using Zenject;
using Managers;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankTargetProvider : MonoBehaviour, IInitializer
    {
        [Inject] private IBoxManager _boxManager;
        [Inject] private ITargetFindService _targetFindService;

        [SerializeField] private Tank _tank;

        private Box _target;
        private TankPlacementAttribute _tankPlacementAttribute;

        public Box Target => _target;

        private bool _waitingForShot;

        public void Initialize()
        {
            _boxManager.OnColumnShifted += OnColumnShifted;
            _tankPlacementAttribute = GetComponent<TankPlacementAttribute>();

            _tank.OnDespawnEvent += OnTankDespawn;
        }

        private bool _isFindingTarget;

        public async void FindTarget()
        {
            if (_waitingForShot) return;

            if (_isFindingTarget) return;

            _isFindingTarget = true;

            try
            {
                var newTarget = await _targetFindService.FindTarget(_tank, transform.position);

                if (newTarget == _target)
                    return;

                if (_target != null)
                {
                    _target.OnDisableEvent -= OnTargetDisabled;
                
                    _target.GetComponent<BoxHitReceiver>().Release();
                }
                
                _target = newTarget;

                if (_target == null)
                    return;

                _target.GetComponentInChildren<BoxHitReceiver>().Reserve();

                _target.OnDisableEvent += OnTargetDisabled;
                _target.OnAdditionalShoot += OnAdditionalShoot;

                _waitingForShot = true;
            }
            finally
            {
                _isFindingTarget = false;
            }
        }

        private void OnTargetDisabled(Box box)
        {
            if (box != _target) return;
            
            if (!gameObject.activeSelf) return;
            
            _target.OnDisableEvent -= OnTargetDisabled;

            FindTarget();
        }

        private void OnAdditionalShoot(Box box)
        {
            if (box != _target) return;

            GetComponent<TankShooter>().Shoot(_target);
        }

        private void OnColumnShifted(Transform column)
        {
            if (_target == null)
            {
                if (_tankPlacementAttribute.IsSetToPlacement)
                {
                    FindTarget();
                }
            }
        }

        public void ResetWaitingForShot()
        {
            _waitingForShot = false;
        }

        private void OnTankDespawn(Tank tank, Transform parentColumn)
        {
            ResetWaitingForShot();

            _tank.OnDespawnEvent -= OnTankDespawn;

            _target.OnDisableEvent -= OnTargetDisabled;
            _target.OnAdditionalShoot -= OnAdditionalShoot;

            _boxManager.OnColumnShifted -= OnColumnShifted;
        }
    }
}