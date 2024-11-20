using UnityEngine;
using System.Collections;

namespace RExt.UI {
    public class SafeAreaException : MonoBehaviour {
        Vector2 defaultScreen = new(1080, 1920);
        RectTransform rectTransform;

        void Awake() {
            ApplyFullArea();
        }

        void ApplyFullArea() {
            rectTransform = GetComponent<RectTransform>();
            StartCoroutine(DoApplyFullArea());
        }

        IEnumerator DoApplyFullArea() {
            yield return new WaitForEndOfFrame();
            FixPosition();
            FixSizeDelta();
        }

        void FixPosition() {
            rectTransform.position = Vector3.zero;
        }

        void FixSizeDelta() {
            var defRatio = defaultScreen.y / defaultScreen.x;
            var scrRatio = (float)Screen.height / Screen.width;
            if (defRatio < scrRatio) {
                rectTransform.sizeDelta = new Vector2(defaultScreen.x, defaultScreen.x * scrRatio);
            } else {
                rectTransform.sizeDelta = new Vector2(defaultScreen.y / scrRatio, defaultScreen.y);
            }
        }
    }
}