using Zenject;
using Managers;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankTargetProvider : MonoBehaviour, IInitializer
    {
        [Inject] private IBoxManager _boxManager;
        [Inject]  private ITargetFindService _targetFindService;

        [SerializeField] private Tank _tank;

        private Box _target;
        private TankPlacementAttribute _tankPlacementAttribute;

        public Box Target => _target;
        
        public void Initialize()
        {
            _boxManager.OnColumnShifted += OnColumnShifted;
            _tankPlacementAttribute = GetComponent<TankPlacementAttribute>();
        }

        private bool _isFindingTarget;

        public async void FindTarget()
        {
            if (_isFindingTarget) return;

            _isFindingTarget = true;

            try
            {
                var newTarget = await _targetFindService.FindTarget(_tank, transform.position);

                if (newTarget == _target)
                    return;

                if (_target != null)
                    _target.OnDisableEvent -= OnTargetDisable;

                _target = newTarget;

                if (_target == null)
                    return;

                _target.GetComponentInChildren<BoxHitReceiver>().Reserve();
                
                _target.OnDisableEvent += OnTargetDisable;
                
                GetComponent<TankShooter>().Shoot(_target);
            }
            finally
            {
                _isFindingTarget = false;
            }
        }

        private void OnTargetDisable(Box box)
        {
            if (box != _target) return;
            
            _target.OnDisableEvent -= OnTargetDisable;
            
            if(!gameObject.activeSelf) return;
            
            FindTarget();
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

        private void OnDisable()
        {
            _boxManager.OnColumnShifted -= OnColumnShifted;
        }
    }
}