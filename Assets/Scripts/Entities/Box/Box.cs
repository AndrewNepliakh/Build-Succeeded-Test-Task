using System;
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
        private Transform _parentColumn;

        public BoxData BoxData => _boxData;
        public GameObject GameObject => gameObject;

        public event Action<Box, Transform> OnDestroy; 
        
        public void Initiate(BoxArguments boxArguments)
        {
            _boxData = boxArguments.BoxData;
            _parentColumn = boxArguments.ParentColumn;

            if (_boxData.Color == BoxColor.None)
            {
                _renderer.sharedMaterial = null;
                return;
            }

            _renderer.sharedMaterial = _materials[(int)_boxData.Color - 1];
        }

        public void OnSpawn()
        {
            transform.localScale = Vector3.one;
        }

        public void OnDespawn()
        {
            OnDestroy?.Invoke(this, _parentColumn);
        }
    }

    public class BoxArguments
    {
        public BoxData BoxData;
        public Transform ParentColumn;
    }
}