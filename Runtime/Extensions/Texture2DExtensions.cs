using UnityEngine;

namespace RExt.Extensions {
    public static class Texture2DExtensions {
        public static Sprite ToSprite(this Texture2D t) {
            if (t == null) return null;
            return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
        }
    }
}