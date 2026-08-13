using System;
using Zenject;
using Services;
using UnityEngine;
using System.Threading.Tasks;
using IPoolable = Services.IPoolable;

namespace Entities
{
    public class BoxDebris : MonoBehaviour, IPoolable
    {
        [Inject] private IPoolService _poolService;
        
        [SerializeField] private ParticleSystem _particleSystem;

        private ParticleSystemRenderer _renderer;

        public GameObject GameObject => gameObject;

        public async void Initialize(Material material)
        {
            _particleSystem.Clear(true);
            _particleSystem.Play(true);
            
            _renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
            _renderer.sharedMaterial = material;

            await Task.Delay(TimeSpan.FromSeconds(3));
            
            _poolService.Despawn(this);
        }

        public void OnSpawn() { }

        public void OnDespawn() { }
    }
}