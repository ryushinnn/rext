using UnityEngine;
using UnityEngine.UI;

namespace RExt.UI {
    public class AutoMatchCanvas : MonoBehaviour {
        [SerializeField] int defaultWidth = 1920;
        [SerializeField] int defaultHeight = 1080;

        void Awake() {
            float currentRatio = (float)Screen.width / Screen.height;
            float defaultRatio = (float)defaultWidth / defaultHeight;
            if (currentRatio > defaultRatio) {
                GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            } else {
                GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            }
        }
    }
}