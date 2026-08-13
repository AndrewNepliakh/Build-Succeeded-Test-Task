using System;
using System.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities
{
    public class BoxHitReceiver : MonoBehaviour, IHitReceiver
    {
        [Inject] private ITargetFindService _targetFindService;
        
        [SerializeField] private Image _indicator;

        private bool _canReceiveHit;
        private bool _isReserved;

        public bool CanReceiveHit => _canReceiveHit;
        public bool IsReserved => _isReserved;
        
        public event Action<IAttackSource> OnHit;

        public void SetCanReceiveTap(bool value)
        {
            _canReceiveHit = value;
            RefreshIndicator();
        }

        public void Release()
        {
            _isReserved = false;
            RefreshIndicator();
        }

        public void Reserve()
        {
            _isReserved = true;
            RefreshIndicator();
        }

        private void RefreshIndicator()
        {
            if (!_canReceiveHit)
                _indicator.color = Color.red;
            else if (_isReserved)
                _indicator.color = Color.yellow;
            else
                _indicator.color = Color.green;
        }

        public void ReceiveHit(IAttackSource attackSource)
        {
            OnHit?.Invoke(attackSource);
        }

        private void OnDisable()
        {
            _canReceiveHit = false;
            _isReserved = false;
            RefreshIndicator();
        }
    }
}