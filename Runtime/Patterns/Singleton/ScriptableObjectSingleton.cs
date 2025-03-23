using UnityEngine;

namespace RExt.Patterns.Singleton {
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject {
        public static T Instance {
            get {
                if (instance == null) {
                    instance = Resources.Load<T>(typeof(T).Name);
                    if (instance == null) {
                        Debug.LogError($"{typeof(T).Name} not found!!!");
                    }
                }
                return instance;
            }
        }
        
        static T instance;
    }
}