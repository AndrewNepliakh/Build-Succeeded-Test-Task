using Zenject;
using Managers;
using Services;
using UnityEngine;

namespace Entities
{
    public class BoxDamageReceiver : MonoBehaviour, IDamageReceiver, ITappable
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Box _box;

        public void OnTap()
        {
            ReceiveDamage();
        }

        public void ReceiveDamage()
        {
            _poolService.Despawn(_box);
        }
    }
}