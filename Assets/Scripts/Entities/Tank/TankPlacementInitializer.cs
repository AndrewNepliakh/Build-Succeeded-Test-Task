using Managers;
using Zenject;
using Services;
using UnityEngine;

namespace Entities
{
    public class TankPlacementInitializer : MonoBehaviour, IInitializer
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
            _tankManager.MoveToPlacement(_tank);
        }

        private void OnDisable()
        {
            _tankTapReceiver.OnTapEvent -= OnTap;
        }
    }
}