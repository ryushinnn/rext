using System.Collections.Generic;
using UnityEngine;

namespace RExt.Utils.ObjectPool {
    public static class ObjectPool {
        static Dictionary<int, GameObject> PrefabsDictionary = new();
        static Dictionary<int, Queue<GameObject>> AvailableObjectDictionary = new();
        static ObjectContainer Container;

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
            if (!AvailableObjectDictionary.ContainsKey(objectTag.PrefabID)) {
                AvailableObjectDictionary.Add(objectTag.PrefabID, new Queue<GameObject>());
            }

            objectTag.IsActive = false;
            obj.SetActive(false);
            AvailableObjectDictionary[objectTag.PrefabID].Enqueue(obj);
            GetContainer().AddToCategory(obj, objectTag.Category);
        }

        static GameObject InstantiateObjectFromPool(GameObject prefab) {
            PrefabsDictionary.TryAdd(prefab.GetInstanceID(), prefab);
            var obj = GetContainer().InstantiateObject(prefab);
            var prefabTag = prefab.GetComponent<ObjectTag>();
            var objectTag = obj.GetComponent<ObjectTag>();
            objectTag.PrefabID = prefab.GetInstanceID();
            objectTag.Category = prefabTag.Category;
            return obj;
        }

        static GameObject GetAvailableObjectFromQueue(GameObject prefab) {
            if (AvailableObjectDictionary.TryGetValue(prefab.GetInstanceID(), out var queue) && queue.Count > 0) {
                return queue.Dequeue();
            }

            return null;
        }

        static GameObject GetNewObject(GameObject prefab) {
            var obj = GetAvailableObjectFromQueue(prefab);
            obj ??= InstantiateObjectFromPool(prefab);
            return obj;
        }

        static ObjectContainer GetContainer() {
            if (!Container) Container = new GameObject("ObjectContainer").AddComponent<ObjectContainer>();
            return Container;
        }

        class ObjectContainer : MonoBehaviour {
            Dictionary<string, Transform> categoryDictionary = new();

            public GameObject InstantiateObject(GameObject prefab) {
                var obj = Instantiate(prefab);
                obj.name = prefab.name;
                return obj;
            }

            public void AddToCategory(GameObject obj, string category) {
                if (!categoryDictionary.ContainsKey(category)) {
                    categoryDictionary.Add(category, CreateCategory(category));
                }
                
                obj.transform.SetParent(categoryDictionary[category]);
            }

            Transform CreateCategory(string category) {
                var tf = new GameObject(category).transform;
                tf.SetParent(transform);
                return tf;
            }
        }
    }
}