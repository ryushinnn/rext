using UnityEngine;

namespace RExt.UI {
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour {
        RectTransform rectTransform;
        Rect lastSafeArea = new(0f, 0f, 0f, 0f);
        ScreenOrientation lastScreenOrientation = ScreenOrientation.AutoRotation;

        void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            Refresh();
        }

        void Refresh() {
            if (lastSafeArea != Screen.safeArea || lastScreenOrientation != Screen.orientation) {
                ApplySafeArea(Screen.safeArea);
            }
        }

        void ApplySafeArea(Rect safeArea) {
            lastSafeArea = safeArea;
            lastScreenOrientation = Screen.orientation;

            if (lastScreenOrientation == ScreenOrientation.LandscapeLeft
                || lastScreenOrientation == ScreenOrientation.LandscapeRight) {
                var anchorMin = safeArea.position;
                var anchorMax = safeArea.position + safeArea.size;

                anchorMin.x /= Screen.width;
                anchorMax.x /= Screen.width;

                rectTransform.anchorMin = new Vector2(anchorMin.x, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(anchorMax.x, rectTransform.anchorMax.y);
            } else if (lastScreenOrientation == ScreenOrientation.Portrait
                       || lastScreenOrientation == ScreenOrientation.PortraitUpsideDown) {
                var anchorMin = safeArea.position;
                var anchorMax = safeArea.position + safeArea.size;

                anchorMin.y /= Screen.height;
                anchorMax.y /= Screen.height;

                rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, anchorMin.y);
                rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, anchorMax.y);
            }
        }
    }
}