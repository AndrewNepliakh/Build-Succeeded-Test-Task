using System.Threading.Tasks;
using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankShooter : MonoBehaviour, IInitializer
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private TankTargetProvider _targetProvider;
        [SerializeField] private Transform _firePoint;

        public Transform FirePoint => _firePoint;

        public void Initialize()
        {
            _targetProvider.OnTargetFound += OnTargetFound;
        }

        private void OnTargetFound(Box target)
        {
            Shoot(target);
        }

        private void Shoot(Box target)
        {
            var projectile = _poolService.Spawn<Projectile>(
                _firePoint.position,
                Quaternion.identity);

            projectile.Initialize(this, target.transform);
        }

        private void OnDisable()
        {
            _targetProvider.OnTargetFound -= OnTargetFound;
        }
    }
}