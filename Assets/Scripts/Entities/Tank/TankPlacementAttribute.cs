using Managers;
using Services;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class TankPlacementAttribute : MonoBehaviour, IAttribute
    {
        [Inject] private IPoolService _poolService;
        [Inject] private ITankManager _tankManager;

        [SerializeField] private Tank _tank;
        [SerializeField] private TankTapReceiver _tankTapReceiver;
        
        public void Initialize()
        {
            _tankTapReceiver.OnTapEvent += OnTap;
        }

        private void OnTap()
        {
            _poolService.Despawn(_tank);
            _tankManager.MoveToPlacement(_tank);
        }
    }
}