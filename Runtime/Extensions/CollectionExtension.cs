using System.Collections.Generic;

namespace Assassin.Extension {
    public static class CollectionExtension {
        public static bool IsNotEmpty<T>(this List<T> list) {
            return list != null && list.Count > 0;
        }
    }
}