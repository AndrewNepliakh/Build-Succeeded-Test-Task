using UnityEngine;

namespace Entities
{
    public class TankTurretAim : MonoBehaviour, IInitializer
    {
        [SerializeField] private TankTargetProvider _targetProvider;
        [SerializeField] private TankShooter _tankShooter;
        [SerializeField] private Transform _turret;
        [SerializeField] private float _rotationSpeed = 720f;
        [SerializeField] private float _aimTolerance = 1f;

        private float _defaultLocalY;
        private Box _lastShotTarget;
        
        private float _targetLostTime;

        public void Initialize()
        {
            _defaultLocalY = _turret.localEulerAngles.y;
            _lastShotTarget = null;
            _targetLostTime = -1f;
        }

        private void Update()
        {
            float targetLocalY = _defaultLocalY;

            if (_targetProvider.Target != null)
            {
                _targetLostTime = Time.time;

                var direction = _targetProvider.Target.transform.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0001f)
                {
                    var worldAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 180f;

                    targetLocalY = Mathf.DeltaAngle(
                        0f,
                        worldAngle - transform.eulerAngles.y);
                }
            }
            else
            {
                _lastShotTarget = null;

                if (Time.time - _targetLostTime < 1.0f)
                    targetLocalY = _turret.localEulerAngles.y;
            }

            var currentY = _turret.localEulerAngles.y;

            var newY = Mathf.MoveTowardsAngle(
                currentY,
                targetLocalY,
                _rotationSpeed * Time.deltaTime);

            _turret.localEulerAngles = new Vector3(-90f, newY, 0f);

            if (_targetProvider.Target == null)
                return;

            if (_lastShotTarget == _targetProvider.Target)
                return;

            var delta = Mathf.Abs(Mathf.DeltaAngle(newY, targetLocalY));

            if (delta <= _aimTolerance)
            {
                _lastShotTarget = _targetProvider.Target;
                _tankShooter.Shoot(_targetProvider.Target);
            }
        }

        private void OnDisable()
        {
            _lastShotTarget = null;
            _turret.localEulerAngles = new Vector3(-90f, _defaultLocalY, 0f);
        }
    }
}