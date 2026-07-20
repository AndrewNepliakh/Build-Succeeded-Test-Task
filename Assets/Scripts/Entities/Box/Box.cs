using Managers;
using Services;
using UnityEngine;

namespace Entities
{
    public class Box : MonoBehaviour, IPoolable
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material[] _materials;
        
        private BoxData _boxData;

        public BoxData BoxData => _boxData;
        public GameObject GameObject => gameObject;
        
        public void Initiate(BoxData boxData)
        {
            _boxData = boxData;

            if (boxData.Color == BoxColor.None)
            {
                _renderer.sharedMaterial = null;
                return;
            }

            _renderer.sharedMaterial = _materials[(int)boxData.Color - 1];
        }

        public void OnSpawn(){}

        public void OnDespawn(){}
    }
}