using System;
using UnityEngine;

namespace RExt.Extensions {
    public static class TransformExtensions {
        public static void ProcessChildren<T>(this Transform t, Action<T> task) {
            foreach (Transform child in t) {
                if (child.TryGetComponent(out T component)) {
                    task?.Invoke(component);
                }
            }
        }
        
        public static void DestroyAllChildren(this Transform t) {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
            }
            else {
                while (t.childCount > 0) {
                    UnityEngine.Object.DestroyImmediate(t.GetChild(0));
                }
            }
#else
            foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
#endif
        }

        public static void SetUniformScale(this Transform t, float factor = 1) {
            t.localScale = factor * Vector3.one;
        }
    }
}