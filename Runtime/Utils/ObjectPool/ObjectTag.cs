using UnityEngine;

namespace RExt.Utils.ObjectPool {
    [DisallowMultipleComponent]
    public class ObjectTag : MonoBehaviour {
        public int PrefabID { get { return prefabID; } set { prefabID = value; } }
        public string Category { get { return category; } set { category = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        [SerializeField] protected string category;
        protected int prefabID;
        protected bool isActive;
    }
}