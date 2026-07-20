using UnityEngine;

namespace Services
{
    public interface IPoolable
    {
        GameObject GameObject { get; }

        void OnSpawn();
        void OnDespawn();
    }
}