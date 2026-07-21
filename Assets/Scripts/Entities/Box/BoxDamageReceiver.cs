using Managers;
using Services;
using UnityEngine;
using Zenject;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class BoxDamageReceiver : MonoBehaviour, IDamageReceiver, ITappable
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Box _box;

        private bool _canReceiveDamage;

        private bool isWasTrue;

        public void SetCanReceiveDamage(bool value)
        {
            _canReceiveDamage = value;
        }

        public void OnTap()
        {
            if (!_canReceiveDamage)
                return;

            ReceiveDamage();
        }

        public void ReceiveDamage()
        {
            _poolService.Despawn(_box);
        }
        
        private void OnDisable()
        {
            _canReceiveDamage = false;
        }
    }
}