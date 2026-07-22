using Services;
using UnityEngine;

namespace Entities
{
    public class Tank : MonoBehaviour, IPoolable
    {
        public GameObject GameObject => gameObject;
        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
    }
}