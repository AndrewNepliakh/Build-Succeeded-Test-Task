using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Managers
{
    public interface IAssetsManager : IDisposable
    {
        Task<T> Instantiate<T>(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Component;

        Task<T> InstantiateUI<T>(
            Transform parent)
            where T : Component;

        void Release(GameObject instance);
    }
}
