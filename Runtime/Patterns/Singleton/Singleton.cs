using System;
using UnityEngine;

namespace RExt.Patterns.Singleton {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get; private set; }
        
        [SerializeField] bool isPersistent;

        void Awake() {
            if (Instance == null) {
                Instance = this as T;
                if (isPersistent) {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
            
            OnAwake();
        }

        protected virtual void OnAwake() {}
    }
}