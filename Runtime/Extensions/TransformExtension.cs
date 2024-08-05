using System;
using UnityEngine;

namespace Ryushin.Extension {
    public static class TransformExtension {
        public static void IterateChildren<T>(this Transform tf, Action<T> task) {
            foreach (Transform child in tf) {
                if (child.TryGetComponent(out T component)) {
                    task?.Invoke(component);
                }
            }
        }
    }
}