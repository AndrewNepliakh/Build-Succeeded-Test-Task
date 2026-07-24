using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class BoxHitReceiver : MonoBehaviour, IHitReceiver, ITappable
    {
        [SerializeField] private Image _indicator;

        private bool _canReceiveHit;
        private bool _isReserved;

        public bool CanReceiveHit => _canReceiveHit;
        public bool IsReserved => _isReserved;

        public void SetCanReceiveTap(bool value)
        {
            _canReceiveHit = value;
            RefreshIndicator();
        }

        public void Reserve()
        {
            _isReserved = true;
            RefreshIndicator();
        }

        public void Release()
        {
            _isReserved = false;
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

        public event Action OnHit;

        public void OnTap()
        {
            if (!_canReceiveHit)
                return;

            ReceiveHit();
        }

        public void ReceiveHit()
        {
            OnHit?.Invoke();
        }

        private void OnDisable()
        {
            _canReceiveHit = false;
            _isReserved = false;
            RefreshIndicator();
        }
    }
}