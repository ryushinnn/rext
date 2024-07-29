using System;
using UnityEngine;

namespace Assassin.Core {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance; 
        
        [SerializeField] private bool _isPersistent;

        public static T Instance() {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    _instance = new GameObject($"{typeof(T)}").AddComponent<T>();
                }
            }

            return _instance;
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this as T;
                if (_isPersistent) {
                    DontDestroyOnLoad(gameObject);
                }
            }else if (_instance != this) {
                Destroy(gameObject);
            }
            
            OnAwake();
        }

        protected virtual void OnAwake() {}
    }
}