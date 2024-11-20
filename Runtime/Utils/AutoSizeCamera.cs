using UnityEngine;

namespace RExt.Utils {
    public class AutoSizeCamera : MonoBehaviour {
        [SerializeField] Vector2 defaultScreen = new(1080, 1920);
        [SerializeField] Orientation orientation;
        [SerializeField] AspectMode aspectMode;

        void Awake() {
            var cam = GetComponent<Camera>();
            if ((orientation == Orientation.Portrait && defaultScreen.x > defaultScreen.y)
                || orientation == Orientation.Landscape && defaultScreen.x < defaultScreen.y) {
                defaultScreen = new Vector2(defaultScreen.y, defaultScreen.x);
            }
            
            var ratio = (1f * Screen.width / Screen.height) / (defaultScreen.x / defaultScreen.y);
            if (aspectMode == AspectMode.Fit) {
                ratio = ratio < 1 ? ratio : 1;
                cam.orthographicSize /= ratio;
            } else if (aspectMode == AspectMode.Envelope) {
                ratio = ratio < 1 ? 1 : ratio;
                cam.orthographicSize /= ratio;
            }
        }

        enum Orientation {
            Portrait,
            Landscape
        }

        enum AspectMode {
            None,
            /// <summary>
            /// Match the smaller edge
            /// </summary>
            Fit,
            /// <summary>
            /// Match the larger edge
            /// </summary>
            Envelope
        }
    }
}