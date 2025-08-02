using UnityEngine;

namespace RExt.Extensions {
    public static class ColorExtensions {
        public static string ToHex(this Color color) {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }   
    }
}