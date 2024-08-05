using System.Collections.Generic;

namespace Ryushin.Extension {
    public static class CollectionExtension {
        public static bool IsNotEmpty<T>(this List<T> list) {
            return list != null && list.Count > 0;
        }

        public static bool IsNotEmpty<T>(this Stack<T> stack) {
            return stack != null && stack.Count > 0;
        }
        
        public static bool IsNotEmpty<T>(this Queue<T> queue) {
            return queue != null && queue.Count > 0;
        }
    }
}