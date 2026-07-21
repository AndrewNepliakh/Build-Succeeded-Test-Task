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

        public T Spawn<T>(Vector3 position, Quaternion rotation, Transform parent = null)
            where T : Component, IPoolable
        {
            var instance = Get<T>(position, rotation);

            var transform = instance.transform;

            transform.SetParent(parent, false);
            transform.SetPositionAndRotation(position, rotation);

            instance.GameObject.SetActive(true);
            instance.OnSpawn();

            return instance;
        }

        public T SpawnUI<T>(Transform parent) where T : Component, IPoolable
        {
            var instance = GetUI<T>();

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

        private T Get<T>(Vector3 position, Quaternion rotation) where T : Component, IPoolable
        {
            var type = typeof(T);

            if (_pool.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    if (obj != null) return (T)obj;
                }
            }

            return _assetsManager.Instantiate<T>(position, rotation);
        }

        private T GetUI<T>() where T : Component, IPoolable
        {
            var type = typeof(T);

            if (_pool.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    if (obj != null) return (T)obj;
                }
            }

            return _assetsManager.Instantiate<T>(Vector3.zero, Quaternion.identity, Root);
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

        public void Clear()
        {
            foreach (var queue in _pool.Values)
            {
                while (queue.Count > 0)
                {
                    var poolable = queue.Dequeue();

                    if (poolable != null)
                        UnityEngine.Object.Destroy(poolable.GameObject);
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

            _assetsManager.ReleaseAll();
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