using UnityEngine;

namespace RExt.Utils.ObjectPool {
    public class ObjectIdentity : MonoBehaviour {
        public string PrefabId => prefabId;
        
        string prefabId;
        
        public void SetId(string prefabId) {
            this.prefabId = prefabId;
        }
    }
}