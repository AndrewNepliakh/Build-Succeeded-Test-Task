using Managers;
using Services;
using UnityEngine;

namespace Entities
{
    public class Box : MonoBehaviour, IPoolable
    {
        private BoxData _boxData;

        public BoxData BoxData => _boxData;
        public GameObject GameObject => gameObject;
        
        public void Init(BoxData boxData)
        {
            _boxData = boxData;
        }

        public void OnSpawn()
        {

        }

        public void OnDespawn()
        {

        }
    }
}