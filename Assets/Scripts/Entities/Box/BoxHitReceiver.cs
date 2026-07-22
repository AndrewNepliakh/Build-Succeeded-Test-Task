using System;
using Managers;
using UnityEngine;

namespace Entities
{
    public class BoxHitReceiver : MonoBehaviour, IHitReceiver, ITappable
    {
        private bool _canReceiveHit;

        public event Action OnHit;

        public void SetCanReceiveHit(bool value)
        {
            _canReceiveHit = value;
        }

        public void OnTap()
        {
            if (!_canReceiveHit) return;

            ReceiveHit();
        }

        public void ReceiveHit()
        {
            OnHit?.Invoke();
        }
        
        private void OnDisable()
        {
            _canReceiveHit = false;
        }
    }
}