using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assassin.Utils.ObjectPool {
    public static class ObjectPool {
        private static Dictionary<int, GameObject> _prefabsDictionary = new();
        private static Dictionary<int, Queue<GameObject>> _availableObjectDictionary = new();
        private static ObjectContainer _container;

        public static GameObject SpawnObject(GameObject prefab, Vector3 position) {
            var obj = GetNewObject(prefab);
            var objectTag = obj.GetComponent<ObjectTag>();
            objectTag.IsActive = true;
            obj.SetActive(true);
            obj.transform.position = position;
            GetContainer().AddToCategory(obj, objectTag.Category);
            return obj;
        }

        public static GameObject SpawnObject(GameObject prefab, Transform parent) {
            var obj = GetNewObject(prefab);
            var objectTag = obj.GetComponent<ObjectTag>();
            objectTag.IsActive = true;
            obj.SetActive(true);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.SetParent(parent);
            return obj;
        }

        public static void DestroyObject(GameObject obj) {
            var objectTag = obj.GetComponent<ObjectTag>();
            if (!objectTag.IsActive) return;
            if (!_availableObjectDictionary.ContainsKey(objectTag.PrefabID)) {
                _availableObjectDictionary.Add(objectTag.PrefabID, new Queue<GameObject>());
            }

            objectTag.IsActive = false;
            obj.SetActive(false);
            _availableObjectDictionary[objectTag.PrefabID].Enqueue(obj);
            GetContainer().AddToCategory(obj, objectTag.Category);
        }

        private static GameObject InstantiateObjectFromPool(GameObject prefab) {
            _prefabsDictionary.TryAdd(prefab.GetInstanceID(), prefab);
            var obj = GetContainer().InstantiateObject(prefab);
            var prefabTag = prefab.GetComponent<ObjectTag>();
            var objectTag = obj.GetComponent<ObjectTag>();
            objectTag.PrefabID = prefab.GetInstanceID();
            objectTag.Category = prefabTag.Category;
            return obj;
        }

        private static GameObject GetAvailableObjectFromQueue(GameObject prefab) {
            if (_availableObjectDictionary.TryGetValue(prefab.GetInstanceID(), out var queue) && queue.Count > 0) {
                return queue.Dequeue();
            }

            return null;
        }

        private static GameObject GetNewObject(GameObject prefab) {
            var obj = GetAvailableObjectFromQueue(prefab);
            obj ??= InstantiateObjectFromPool(prefab);
            return obj;
        }

        private static ObjectContainer GetContainer() {
            if (!_container) _container = new GameObject("ObjectContainer").AddComponent<ObjectContainer>();
            return _container;
        }

        private class ObjectContainer : MonoBehaviour {
            private Dictionary<Category, Transform> _categoryDictionary = new();

            public GameObject InstantiateObject(GameObject prefab) {
                var obj = Instantiate(prefab);
                obj.name = prefab.name;
                return obj;
            }

            public void AddToCategory(GameObject obj, Category category) {
                if (!_categoryDictionary.ContainsKey(category)) {
                    _categoryDictionary.Add(category, CreateCategory(category));
                }
                
                obj.transform.SetParent(_categoryDictionary[category]);
            }

            private Transform CreateCategory(Category category) {
                var tf = new GameObject(category.ToString()).transform;
                tf.SetParent(transform);
                return tf;
            }
        }
    }
}