using UnityEngine;

namespace RExt.Extensions {
    public static class VectorExtensions {
        public static Vector3 ToZeroY(this Vector3 v) {
            return new Vector3(v.x, 0, v.z);
        }
    }
}