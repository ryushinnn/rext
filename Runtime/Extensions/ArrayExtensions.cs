using System;

namespace RExt.Extensions {
    public static class ArrayExtensions {
        public static T Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find(array, match);
        }

        public static void Sort<T>(this T[] array, Comparison<T> comparison) {
            Array.Sort(array, comparison);
        }
        
        public static bool IsNotEmpty<T>(this T[] array) {
            return array != null && array.Length > 0;
        }
    }
}

