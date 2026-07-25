using Zenject;
using Managers;
using UnityEngine;

namespace Entities
{
    public class TankTargetProvider : MonoBehaviour, IInitializer
    {
        [Inject] private IBoxManager _boxManager;

        [SerializeField] private Tank _tank;

        private Box _target;
        private TankPlacementAttribute _tankPlacementAttribute;

        public Box Target => _target;
        
        public void Initialize()
        {
            _boxManager.OnColumnShifted += OnColumnShifted;
            _tankPlacementAttribute = GetComponent<TankPlacementAttribute>();
        }

        public void FindTarget()
        {
            Box newTarget = null;
            var bestDistance = float.MaxValue;

            foreach (var column in _boxManager.GetColumns())
            {
                foreach (Transform child in column.transform)
                {
                    if (!child.TryGetComponent(out Box box))
                        continue;
                    
                    var hitReceiver = box.GetComponentInChildren<BoxHitReceiver>();

                    if (!hitReceiver.CanReceiveHit)
                        continue;
                    
                    if (hitReceiver.IsReserved)
                        continue;
                    
                    if (box.BoxData.Color != _tank.TankData.Color)
                        continue;
                    
                    var distance = (box.transform.position - transform.position).sqrMagnitude;

                    if (distance >= bestDistance)
                        continue;

                    bestDistance = distance;
                    newTarget = box;
                }
            }
            
            if (newTarget != null && newTarget == _target)
                return;
            
            _target = newTarget;
            
            if (_target == null) return;
            
            _target.GetComponentInChildren<BoxHitReceiver>().Reserve();
            
            _target.OnDisableEvent += OnTargetDisable;
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