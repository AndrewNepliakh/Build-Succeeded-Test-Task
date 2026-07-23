using UnityEngine;

namespace Entities
{
    public class TankPlacement : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;

        public Transform Pivot => _pivot;
    }
}