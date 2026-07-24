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
        }

        public void Release(Tank tank)
        {
            if (_currentTank == tank)
                _currentTank = null;
        }
    }
}