using UnityEngine;

namespace Assassin.Utils.ObjectPool {
    public enum Category {
        None,
        VFX,
        Projectile
    }

    [DisallowMultipleComponent]
    public class ObjectTag : MonoBehaviour {
        public int PrefabID { get { return _prefabID; } set { _prefabID = value; } }
        public Category Category { get { return _category; } set { _category = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        [SerializeField] private Category _category;
        private int _prefabID;
        private bool _isActive;
    }
}