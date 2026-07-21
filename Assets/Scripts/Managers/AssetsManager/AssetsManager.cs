using System;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class AssetsManager : IAssetsManager
    {
        [Inject] private readonly DiContainer _diContainer;

        private readonly Dictionary<Type, AsyncOperationHandle<GameObject>> _loadedPrefabs = new();

        public async Task PreloadAssetAsync<T>() where T : Component
        {
            await GetPrefab<T>();
        }

        public async Task<GameObject> GetPrefab<T>() where T : Component
        {
            var type = typeof(T);

            if (_loadedPrefabs.TryGetValue(type, out var handle))
            {
                if (handle.IsValid()) return handle.Result;

                _loadedPrefabs.Remove(type);
            }

            var address = AssetsAddresses.Get<T>();

            handle = Addressables.LoadAssetAsync<GameObject>(address);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load prefab '{address}'.");

            _loadedPrefabs[type] = handle;

            return handle.Result;
        }

        public T Instantiate<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            var type = typeof(T);

            if (!_loadedPrefabs.TryGetValue(type, out var handle) || !handle.IsValid())
                throw new InvalidOperationException(
                    $"Prefab '{type.Name}' is not preloaded.");

            var gameObject = _diContainer.InstantiatePrefab(
                handle.Result,
                position,
                rotation,
                parent);

            return gameObject.GetComponent<T>();
        }

        public void Release<T>() where T : Component
        {
            var type = typeof(T);

            if (!_loadedPrefabs.TryGetValue(type, out var handle))
                return;

            if (handle.IsValid())
                Addressables.Release(handle);

            _loadedPrefabs.Remove(type);
        }

        public void ReleaseAll()
        {
            foreach (var handle in _loadedPrefabs.Values)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }

            _loadedPrefabs.Clear();
        }
    }
}