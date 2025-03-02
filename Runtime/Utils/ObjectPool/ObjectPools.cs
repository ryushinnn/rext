using System;
using System.Collections.Generic;
using RExt.Core;
using UnityEngine;

namespace RExt.Utils.ObjectPool {
    public class ObjectPools : Singleton<ObjectPools> {
        Dictionary<Type, object> pools = new();
        
        public ObjectPool<T> GetPool<T>(T prefab, int initialSize = 5, int maxSize = 20, int expansionSize = 5) where T : MonoBehaviour, IPoolable {
            var type = typeof(T);
            if (pools.TryGetValue(type, out var pool)) {
                return pool as ObjectPool<T>;
            }
            
            var newPool = new ObjectPool<T>(prefab, initialSize, maxSize, expansionSize, transform);
            pools.Add(type, newPool);
            return newPool;
        }
    }
}