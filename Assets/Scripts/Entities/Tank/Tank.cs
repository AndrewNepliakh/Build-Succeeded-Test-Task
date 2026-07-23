using System;
using Services;
using UnityEngine;
using Sirenix.Utilities;
using Object = UnityEngine.Object;

namespace Entities
{
    public class Tank : MonoBehaviour, IPoolable
    {
        private TankData _tankData;
        private Transform _parentColumn;
        
        public TankData TankData => _tankData;
        public GameObject GameObject => gameObject;
        public Transform ParentColumn => _parentColumn;
        
        public event Action<Tank, Transform> OnDespawnEvent; 
        
        public void Initiate(TankArguments tankArguments)
        {
            _tankData = tankArguments.TankData;
            _parentColumn = tankArguments.ParentColumn;

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
    
    public class TankArguments
    {
        public TankData TankData;
        public Transform ParentColumn;
    }
}