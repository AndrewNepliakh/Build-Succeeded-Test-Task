using System;
using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Services
{
    public class PoolService : IPoolService
    {
        [Inject] private IAssetsManager _assetsManager;

        private readonly Dictionary<Type, Queue<IPoolable>> _pool = new();
        private readonly Dictionary<Type, Transform> _parents = new();
        private readonly Dictionary<Type, HashSet<IPoolable>> _registered = new();

        private Transform _root;

        private Transform Root
        {
            get
            {
                if (_root == null)
                {
                    _root = new GameObject("Pool").transform;
                }

                return _root;
            }
        }

        public async Task<T> Spawn<T>(
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
            where T : Component, IPoolable
        {
            var instance = await Get<T>();

            var transform = instance.transform;

            transform.SetParent(parent, false);
            transform.SetPositionAndRotation(position, rotation);

            instance.GameObject.SetActive(true);
            instance.OnSpawn();

            return instance;
        }

        public async Task<T> SpawnUI<T>(
            Transform parent)
            where T : Component, IPoolable
        {
            var instance = await GetUI<T>();

            var transform = instance.transform;
            transform.SetParent(parent, false);

            if (transform is RectTransform rect)
            {
                rect.localPosition = Vector3.zero;
                rect.localRotation = Quaternion.identity;
                rect.localScale = Vector3.one;
            }

            instance.GameObject.SetActive(true);
            instance.OnSpawn();

            return instance;
        }

        private async Task<T> Get<T>()
            where T : Component, IPoolable
        {
            var type = typeof(T);

            if (_pool.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();

                    if (obj != null)
                        return (T)obj;
                }
            }

            return await _assetsManager.Instantiate<T>(Vector3.zero, Quaternion.identity);
        }

        private async Task<T> GetUI<T>()
            where T : Component, IPoolable
        {
            var type = typeof(T);

            if (_pool.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();

                    if (obj != null)
                        return (T)obj;
                }
            }

            return await _assetsManager.InstantiateUI<T>(Root);
        }

        public void Despawn(IPoolable poolable)
        {
            var type = poolable.GetType();

            if (!_pool.TryGetValue(type, out var queue))
            {
                queue = new Queue<IPoolable>();
                _pool[type] = queue;
            }

            poolable.OnDespawn();

            var parent = GetPoolParent(type);

            var transform = poolable.GameObject.transform;
            transform.SetParent(parent, false);

            poolable.GameObject.SetActive(false);

            queue.Enqueue(poolable);
        }

        public void Destroy(IPoolable poolable)
        {
            if (poolable == null)
                return;

            _assetsManager.Release(poolable.GameObject);
        }
        
        public void Register(IPoolable poolable)
        {
            var type = poolable.GetType();

            if (!_registered.TryGetValue(type, out var set))
            {
                set = new HashSet<IPoolable>();
                _registered[type] = set;
            }

            set.Add(poolable);
        }

        public void Clear()
        {
            foreach (var queue in _pool.Values)
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();

                    if (obj != null)
                        _assetsManager.Release(obj.GameObject);
                }
            }

            _pool.Clear();

            foreach (var parent in _parents.Values)
            {
                if (parent != null)
                {
                    UnityEngine.Object.Destroy(parent.gameObject);
                }
            }

            _parents.Clear();

            if (_root != null)
            {
                UnityEngine.Object.Destroy(_root.gameObject);
                _root = null;
            }
        }

        private Transform GetPoolParent(Type type)
        {
            if (_parents.TryGetValue(type, out var parent))
                return parent;

            parent = new GameObject(type.Name).transform;
            parent.SetParent(Root);

            _parents[type] = parent;

            return parent;
        }
    }
}