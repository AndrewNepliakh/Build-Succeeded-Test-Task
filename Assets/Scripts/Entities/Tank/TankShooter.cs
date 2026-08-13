using TMPro;
using System;
using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankShooter : MonoBehaviour, IInitializer, IAttackSource
    {
        public static event Action OnShootStatic;
        
        [Inject] private IPoolService _poolService;

        [SerializeField] private Tank _tank;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private TMP_Text _indicatorText;

        private int _shotsLeft;

        public Transform FirePoint => _firePoint;
        public GameObject GameObject => gameObject;
        
        public event Action OnShootEvent;
        public event Action OnDisableEvent;

        public void Initialize()
        {
            _shotsLeft = _tank.TankData.ShootsCount;
            _indicatorText.text = _shotsLeft.ToString();
        }

        public void Shoot(Box target)
        {
            if (target == null)
                return;

            if (_shotsLeft <= 0)
                return;

            _shotsLeft--;
            
            _indicatorText.text = _shotsLeft.ToString();

            var projectile = _poolService.Spawn<Projectile>(
                _firePoint.position,
                Quaternion.identity);

            projectile.Initialize(this, target.transform);
            
            OnShootEvent?.Invoke();
            
            OnShootStatic?.Invoke();

            if (_shotsLeft == 0)
            {
                _poolService.Despawn(_tank);
            }
        }

        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
    }
}