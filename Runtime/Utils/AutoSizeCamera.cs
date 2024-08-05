using UnityEngine;

namespace Ryushin.Utils {
    public class AutoSizeCamera : MonoBehaviour {
        [SerializeField] private Vector2 _defaultScreen = new(1080, 1920);
        [SerializeField] private Orientation _orientation;
        [SerializeField] private AspectMode _aspectMode;

        private void Awake() {
            var cam = GetComponent<Camera>();
            if ((_orientation == Orientation.Portrait && _defaultScreen.x > _defaultScreen.y)
                || _orientation == Orientation.Landscape && _defaultScreen.x < _defaultScreen.y) {
                _defaultScreen = new Vector2(_defaultScreen.y, _defaultScreen.x);
            }
            
            var ratio = (1f * Screen.width / Screen.height) / (_defaultScreen.x / _defaultScreen.y);
            if (_aspectMode == AspectMode.Fit) {
                ratio = ratio < 1 ? ratio : 1;
                cam.orthographicSize /= ratio;
            } else if (_aspectMode == AspectMode.Envelope) {
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