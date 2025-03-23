using System;
using System.Collections.Generic;
using UnityEngine;

namespace RExt.Patterns.ObjectPool {
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable {
        public int ActiveCount => activeCount;
        public int TotalCount => activeCount + availableObjects.Count;
        
        readonly Queue<T> availableObjects = new();
        readonly LinkedList<T> activeObjects = new();
        readonly T prefab;
        readonly Transform parent;
        readonly int initialSize;
        readonly int maxSize;
        readonly int expansionSize;
        
        int activeCount = 0;

        public ObjectPool(T prefab, int initialSize, int maxSize, int expansionSize, Transform parent = null) {
            this.prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            this.initialSize = Mathf.Max(initialSize, 1);
            this.maxSize = Mathf.Max(maxSize, this.initialSize);
            this.expansionSize = Mathf.Max(expansionSize, 1);
            this.parent = parent;
            
            Initialize();
        }

        public T Get() {
            T obj;
            if (availableObjects.Count > 0) {
                obj = availableObjects.Dequeue();
            }
            else if (activeCount < maxSize) {
                Expand();
                obj = availableObjects.Dequeue();
            }
            else {
                obj = RecycleOldestObject();
            }

            obj.Activate();
            activeObjects.AddLast(obj);
            activeCount++;
            return obj;
        }

        public void Return(T obj) {
            if (obj == null) return;
            
            obj.Deactivate();
            activeObjects.Remove(obj);
            availableObjects.Enqueue(obj);
            activeCount--;
        }

        void Initialize() {
            for (int i = 0; i < initialSize; i++) {
                var obj = CreateNewObject();
                obj.Deactivate();
                availableObjects.Enqueue(obj);
            }
        }

        T CreateNewObject() {
            var obj = UnityEngine.Object.Instantiate(prefab, parent);
            return obj;
        }

        T RecycleOldestObject() {
            var obj = activeObjects.First.Value;
            activeObjects.RemoveFirst();
            obj.Deactivate();
            return obj;
        }

        void Expand() {
            var expandCount = Mathf.Min(expansionSize, maxSize - (availableObjects.Count + activeCount));
            for (int i = 0; i < expandCount; i++) {
                var obj = CreateNewObject();
                obj.Deactivate();
                availableObjects.Enqueue(obj);
            }
        }

        void Cleanup(int targetSize) {
            targetSize = Mathf.Clamp(targetSize, 0, maxSize);
            while (availableObjects.Count > targetSize && availableObjects.Count + activeCount > initialSize) {
                var obj = availableObjects.Dequeue();
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj.gameObject);
                }
            }
        }

        void Clear() {
            while (availableObjects.Count > 0) {
                var obj = availableObjects.Dequeue();
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj.gameObject);
                }
            }

            foreach (var obj in activeObjects) {
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj.gameObject);
                }
            }

            activeObjects.Clear();
            activeCount = 0;
        }
    }

    public class ObjectPool {
        public int ActiveCount => activeCount;
        public int TotalCount => activeCount + availableObjects.Count;
        
        readonly Queue<GameObject> availableObjects = new();
        readonly LinkedList<GameObject> activeObjects = new();
        readonly GameObject prefab;
        readonly Transform parent;
        readonly int initialSize;
        readonly int maxSize;
        readonly int expansionSize;
        readonly string prefabId;
        
        int activeCount = 0;

        public ObjectPool(GameObject prefab, int initialSize, int maxSize, int expansionSize, Transform parent = null) {
            this.prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            this.initialSize = Mathf.Max(initialSize, 1);
            this.maxSize = Mathf.Max(maxSize, this.initialSize);
            this.expansionSize = Mathf.Max(expansionSize, 1);
            this.parent = parent;
            prefabId = prefab.GetInstanceID().ToString();
            
            Initialize();
        }

        public GameObject Get() {
            GameObject obj;
            if (availableObjects.Count > 0) {
                obj = availableObjects.Dequeue();
            }
            else if (activeCount < maxSize) {
                Expand();
                obj = availableObjects.Dequeue();
            }
            else {
                obj = RecycleOldestObject();
            }

            obj.SetActive(true);
            activeObjects.AddLast(obj);
            activeCount++;
            return obj;
        }

        public void Return(GameObject obj) {
            if (obj == null) return;
            
            obj.SetActive(false);
            activeObjects.Remove(obj);
            availableObjects.Enqueue(obj);
            activeCount--;
        }

        void Initialize() {
            for (int i = 0; i < initialSize; i++) {
                var obj = CreateNewObject();
                obj.SetActive(false);
                availableObjects.Enqueue(obj);
            }
        }
        
        GameObject CreateNewObject() {
            var obj = UnityEngine.Object.Instantiate(prefab, parent);
            obj.AddComponent<ObjectIdentity>().SetId(prefabId);
            return obj;
        }
        
        GameObject RecycleOldestObject() {
            var obj = activeObjects.First.Value;
            activeObjects.RemoveFirst();
            obj.SetActive(false);
            return obj;
        }
        
        void Expand() {
            var expandCount = Mathf.Min(expansionSize, maxSize - (availableObjects.Count + activeCount));
            for (int i = 0; i < expandCount; i++) {
                var obj = CreateNewObject();
                obj.SetActive(false);
                availableObjects.Enqueue(obj);
            }
        }
        
        void Cleanup(int targetSize) {
            targetSize = Mathf.Clamp(targetSize, 0, maxSize);
            while (availableObjects.Count > targetSize && availableObjects.Count + activeCount > initialSize) {
                var obj = availableObjects.Dequeue();
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj);
                }
            }
        }
        
        void Clear() {
            while (availableObjects.Count > 0) {
                var obj = availableObjects.Dequeue();
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj);
                }
            }

            foreach (var obj in activeObjects) {
                if (obj != null) {
                    UnityEngine.Object.Destroy(obj);
                }
            }

            activeObjects.Clear();
            activeCount = 0;
        }
    }
}