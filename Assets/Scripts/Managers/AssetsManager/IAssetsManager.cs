using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public interface IAssetsManager
    {
        Task PreloadAssetAsync<T>() where T : Component;

        T Instantiate<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : Component;

        Task<GameObject> GetPrefab<T>() where T : Component;

        void Release<T>() where T : Component;

        void ReleaseAll();
    }
}