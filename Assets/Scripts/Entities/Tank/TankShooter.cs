using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankShooter : MonoBehaviour
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Transform _firePoint;

        public Transform FirePoint => _firePoint;

        public void Shoot(Box target)
        {
            if (target == null)
                return;

            var projectile = _poolService.Spawn<Projectile>(
                _firePoint.position,
                Quaternion.identity);

            projectile.Initialize(this, target.transform);
        }
    }
}