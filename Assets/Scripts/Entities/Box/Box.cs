using System;
using Managers;
using Services;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entities
{
    public class Box : MonoBehaviour, IPoolable
    {
        private BoxData _boxData;
        private Transform _parentColumn;
        
        public BoxData BoxData => _boxData;
        public GameObject GameObject => gameObject;
        public Object ParentColumn => _parentColumn;
        
        public event Action<Box, Transform> OnDespawnEvent; 
        
        public void Initiate(BoxArguments boxArguments)
        {
            _boxData = boxArguments.BoxData;
            _parentColumn = boxArguments.ParentColumn;

            GetComponentsInChildren<IAttribute>().ForEach(x => x.Initialize());
        }

        public void OnSpawn()
        {
            transform.localScale = Vector3.one;
        }

        public void OnDespawn()
        {
            OnDespawnEvent?.Invoke(this, _parentColumn);
        }
    }

    public class BoxArguments
    {
        public BoxData BoxData;
        public Transform ParentColumn;
    }
}