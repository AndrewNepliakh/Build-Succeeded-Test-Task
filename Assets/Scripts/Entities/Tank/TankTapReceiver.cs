using System;
using Managers;
using UnityEngine;

namespace Entities
{
    public class TankTapReceiver : MonoBehaviour, ITappable
    {
        private bool _canReceiveTap;

        public event Action OnTapEvent;

        public void SetCanReceiveTap(bool value)
        {
            _canReceiveTap = value;
        }

        public void OnTap()
        {
            if (!_canReceiveTap) return;

            OnTapEvent?.Invoke();
        }
        
        private void OnDisable()
        {
            _canReceiveTap = false;
        }
    }
}