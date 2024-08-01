using UnityEngine;

namespace Assassin.UI {
    public class UI : MonoBehaviour {
        public virtual void Open(params object[] prs) {
            gameObject.SetActive(true);
        }

        public virtual void Close() {
            gameObject.SetActive(false);
        }
    }
}