using Zenject;
using Managers;
using UnityEngine;

namespace Entities
{
    public class TankPlacementAttribute : MonoBehaviour, IInitializer
    {
        [Inject] private ITankManager _tankManager;

        [SerializeField] private Tank _tank;
        [SerializeField] private TankTapReceiver _tankTapReceiver;
        private TankPlacement _placement;
        
        public void Initialize()
        {
            _tankTapReceiver.OnTapEvent += OnTap;
        }
        
        public void SetPlacement(TankPlacement placement)
        {
            _placement = placement;
        }

        private void OnTap()
        {
            _tankManager.MoveToPlacement(_tank);
        }

        private void OnDisable()
        {
            _tankTapReceiver.OnTapEvent -= OnTap;

            if (_placement != null)
            {
                _placement.Release(_tank);
                _placement = null;
            }
        }
    }
}