using UnityEngine;

namespace RExt.Utils {
    public static class RLog {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context = null)
        {
            if (context) {
                Debug.Log(message, context);
            } else {
                Debug.Log(message);
            }
        }
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context = null)
        {
            if (context) {
                Debug.LogError(message, context);
            } else {
                Debug.LogError(message);
            }
        }
    }
}