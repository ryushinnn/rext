using UnityEngine;

namespace Assassin.Utils {
    public static class Logger {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            Debug.Log(message);
        }
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object context)
        {
            Debug.Log(message, context);
        }
    }
}