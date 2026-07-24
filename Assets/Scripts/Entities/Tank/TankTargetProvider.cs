using System;
using Zenject;
using Managers;
using UnityEngine;

namespace Entities
{
    public class TankTargetProvider : MonoBehaviour
    {
        [Inject] private IBoxManager _boxManager;

        [SerializeField] private Tank _tank;

        private Box _target;

        public Box Target => _target;

        public event Action<Box> OnTargetFound;

        public void StartSearchTarget()
        {
            CancelInvoke(nameof(FindTarget));

            InvokeRepeating(nameof(FindTarget), 0f, 0.15f);
        }

        private void FindTarget()
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

            if (newTarget == _target)
                return;

            if (_target != null)
                _target.GetComponentInChildren<BoxHitReceiver>().Release();

            _target = newTarget;

            if (_target == null)
                return;

            _target.GetComponentInChildren<BoxHitReceiver>().Reserve();

            OnTargetFound?.Invoke(_target);
        }

        public void StopSearchTarget()
        {
            CancelInvoke(nameof(FindTarget));
            _target = null;
        }

        private void OnDisable()
        {
            StopSearchTarget();
        }
    }
}