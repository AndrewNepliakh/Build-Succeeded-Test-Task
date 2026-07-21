using UnityEngine;
using System.Threading.Tasks;

namespace Services
{
    public interface IPoolService
    {
        T Spawn<T>(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Component, IPoolable;

        T SpawnUI<T>(Transform parent) where T : Component, IPoolable;

        void Despawn(IPoolable poolable);

        void Clear();
    }
}