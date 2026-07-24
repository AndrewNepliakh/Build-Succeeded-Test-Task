using UnityEngine;

namespace Entities
{
    public class TankPlacement : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;

        private Tank _currentTank;

        public Transform Pivot => _pivot;
        public bool IsOccupied => _currentTank != null;

        public void Occupy(Tank tank)
        {
            _currentTank = tank;
            
            _currentTank.GetComponent<TankTargetProvider>().StartSearchTarget();
        }

        public void Release()
        {
            _currentTank = null;
        }
    }
}