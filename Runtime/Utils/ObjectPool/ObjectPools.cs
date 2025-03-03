using System;
using System.Collections.Generic;
using RExt.Core;
using UnityEngine;

namespace RExt.Utils.ObjectPool {
    public class ObjectPools : Singleton<ObjectPools> {
        Dictionary<Type, object> pools = new();
        Dictionary<string, ObjectPool> genericPools = new();
        
        const int DEFAULT_INITIAL_SIZE = 5;
        const int DEFAULT_MAX_SIZE = 20;
        const int DEFAULT_EXPANSION_SIZE = 5;
        
        public ObjectPool<T> CreatePool<T>(T prefab, int initialSize = DEFAULT_INITIAL_SIZE, int maxSize = DEFAULT_MAX_SIZE, int expansionSize = DEFAULT_EXPANSION_SIZE) where T : MonoBehaviour, IPoolable {
            var type = typeof(T);
            if (pools.TryGetValue(type, out var pool)) {
                return pool as ObjectPool<T>;
            }
            
            var newPool = new ObjectPool<T>(prefab, initialSize, maxSize, expansionSize, transform);
            pools.Add(type, newPool);
            return newPool;
        }

        public T Get<T>(T prefab) where T : MonoBehaviour, IPoolable {
            var type = typeof(T);
            if (pools.TryGetValue(type, out var pool)) {
                return ((ObjectPool<T>)pool).Get();
            }
            
            return CreatePool(prefab).Get();
        }

        public void Return<T>(T obj) where T : MonoBehaviour, IPoolable {
            var type = typeof(T);
            if (pools.TryGetValue(type, out var pool)) {
                ((ObjectPool<T>)pool).Return(obj);
            }
        }
        
        public ObjectPool CreatePool(GameObject prefab, int initialSize = DEFAULT_INITIAL_SIZE, int maxSize = DEFAULT_MAX_SIZE, int expansionSize = DEFAULT_EXPANSION_SIZE) {
            var key = prefab.GetInstanceID().ToString();
            if (genericPools.TryGetValue(key, out var pool)) {
                return pool;
            }
            
            var newPool = new ObjectPool(prefab, initialSize, maxSize, expansionSize, transform);
            genericPools.Add(key, newPool);
            return newPool;
        }

        public GameObject Get(GameObject prefab) {
            var key = prefab.GetInstanceID().ToString();
            if (genericPools.TryGetValue(key, out var pool)) {
                return pool.Get();
            }
            
            return CreatePool(prefab).Get();
        }

        public void Return(GameObject obj) {
            if (!obj.TryGetComponent(out ObjectIdentity id)) return;

            var key = id.PrefabId;
            if (genericPools.TryGetValue(key, out var pool)) {
                pool.Return(obj);
            }
        }
    }
}