using UnityEngine;
using UnityEngine.UI;

namespace Ryushin.UI {
    public class AutoMatchCanvas : MonoBehaviour {
        [SerializeField] private int _defaultWidth = 1920;
        [SerializeField] private int _defaultHeight = 1080;

        private void Awake() {
            float currentRatio = (float)Screen.width / Screen.height;
            float defaultRatio = (float)_defaultWidth / _defaultHeight;
            if (currentRatio > defaultRatio) {
                GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            } else {
                GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            }
        }
    }
}