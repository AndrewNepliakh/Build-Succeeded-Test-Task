using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class AssetsManager : IAssetsManager
    {
        [Inject] private readonly DiContainer _diContainer;

        public async Task<T> Instantiate<T>(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Component
        {
            var handle = Addressables.InstantiateAsync(AssetsAddresses.Get<T>(), position, rotation, parent);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to instantiate addressable: {AssetsAddresses.Get<T>()}");
                return null;
            }

            _diContainer.InjectGameObject(handle.Result);

            return handle.Result.GetComponent<T>();
        }

        public async Task<T> InstantiateUI<T>(
            Transform parent)
            where T : Component
        {
            var handle = Addressables.InstantiateAsync(AssetsAddresses.Get<T>(), parent);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to instantiate addressable: {AssetsAddresses.Get<T>()}");
                return null;
            }

            var instance = handle.Result;

            _diContainer.InjectGameObject(instance);

            var rectTransform = instance.GetComponent<RectTransform>();
            
            if (rectTransform != null)
            {
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localScale = Vector3.one;
            }

            return instance.GetComponent<T>();
        }

        public void Release(GameObject instance)
        {
            if (instance != null)
            {
                Addressables.ReleaseInstance(instance);
            }
        }

        public void Dispose() => Addressables.ClearResourceLocators();
    }
}