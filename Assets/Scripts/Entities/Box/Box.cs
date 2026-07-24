using System;
using Zenject;
using Managers;
using UnityEngine;
using Sirenix.Utilities;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class Box : MonoBehaviour, IPoolable
    {
        public static event Action OnDespawnStatic;
        
        [Inject] private IBoxManager _boxManager;
        
        private BoxData _boxData;
        private Transform _parentColumn;
        
        public BoxData BoxData => _boxData;
        public GameObject GameObject => gameObject;
        public Transform ParentColumn => _parentColumn;
        
        public event Action<Box> OnDespawnEvent; 
        
        public void Initiate(BoxArguments boxArguments)
        {
            _boxData = boxArguments.BoxData;
            _parentColumn = boxArguments.ParentColumn;

            GetComponentsInChildren<IInitializer>().ForEach(x => x.Initialize());
        }

        public void OnSpawn()
        {
            transform.localScale = Vector3.one;
        }

        public void OnDespawn()
        {
            OnDespawnEvent?.Invoke(this);
            OnDespawnStatic?.Invoke();
        }
    }

    public class BoxArguments
    {
        public BoxData BoxData;
        public Transform ParentColumn;
    }
}