using Zenject;
using Services;
using UnityEngine;
using System.Collections.Generic;

namespace Entities
{
    public class BoxDebrisAttribute : MonoBehaviour, IInitializer
    {
        [Inject] private IPoolService _poolService;

        [SerializeField] private Box _box;
        [SerializeField] private List<Material> _materials = new();

        public void Initialize()
        {
            _box.OnDespawnEvent += OnBoxDespawnEvent;
        }

        private void OnBoxDespawnEvent(Box box)
        {
            if (_box != box) return;

            if (_box.BoxData.Color != BoxColor.None)
            {
                var material = _materials[(int)_box.BoxData.Color - 1];
                var debris = _poolService.Spawn<BoxDebris>(transform.position, Quaternion.identity);
                debris.Initialize(material);
            }
            
            _box.OnDespawnEvent -= OnBoxDespawnEvent;
        }
    }
}