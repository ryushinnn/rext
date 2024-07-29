using UnityEngine;

namespace Assassin.Utils.ObjectPool {
    [DisallowMultipleComponent]
    public class ObjectTag : MonoBehaviour {
        public int PrefabID { get { return _prefabID; } set { _prefabID = value; } }
        public string Category { get { return _category; } set { _category = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        [SerializeField] private string _category;
        private int _prefabID;
        private bool _isActive;
    }
}