using UnityEngine;

namespace RExt.Extensions {
    public static class LayerExtensions {
        public static bool Contains(this LayerMask layerMask, int layer) {
            return (layerMask & (1 << layer)) != 0;
        }
    }
}