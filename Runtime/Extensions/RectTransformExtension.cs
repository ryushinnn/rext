using UnityEngine;

namespace Ryushin.Extension {
    public static class RectTransformExtension {
        public static void SetRect(this RectTransform rt, float left, float right, float top, float bottom) {
            rt.SetLeft(left);
            rt.SetRight(right);
            rt.SetTop(top);
            rt.SetBottom(bottom);
        }

        public static void SetLeft(this RectTransform rt, float left) {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right) {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top) {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom) {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
        
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            for (var i = 0; i < objectCorners.Length; i++) {
                var tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);
                if (screenBounds.Contains(tempScreenSpaceCorner))
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }
        
        /// <summary>
        /// Check if a RectTransform is FULLY visible in screen
        /// </summary>
        /// <param name="rectTransform">target</param>
        /// <param name="camera">seen by which camera</param>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) == 4;
        }
        
        /// <summary>
        /// Check if a RectTransform is visible in screen
        /// </summary>
        /// <param name="rectTransform">target</param>
        /// <param name="camera">seen by which camera</param>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) > 0;
        }
        
        /// <summary>
        /// Check if a RectTransform is visible in another RectTransform (e.g. a ScrollRect)
        /// </summary>
        /// <param name="element">target</param>
        /// <param name="container">container</param>
        public static bool IsVisibleInside(this RectTransform element, RectTransform container)
        {
            var elementCorners = new Vector3[4];
            var containerCorners = new Vector3[4];
        
            element.GetWorldCorners(elementCorners);
            container.GetWorldCorners(containerCorners);

            var isVisible = false;
            foreach (Vector3 corner in elementCorners)
            {
                if (corner.x >= containerCorners[0].x && corner.x <= containerCorners[2].x &&
                    corner.y >= containerCorners[0].y && corner.y <= containerCorners[2].y)
                {
                    isVisible = true;
                    break;
                }
            }

            if (!isVisible)
            {
                var rect1 = new Rect(elementCorners[0], element.sizeDelta);
                var rect2 = new Rect(containerCorners[0], container.sizeDelta);
                isVisible = rect1.Overlaps(rect2);
            }

            return isVisible;
        }
    }
}