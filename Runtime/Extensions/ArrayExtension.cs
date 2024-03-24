using System;
using System.Collections.Generic;

namespace Assassin.Extension {
    public static class ArrayExtension {
        public static T Find<T>(this T[] array, Predicate<T> match) {
            foreach (var element in array) {
                if (match(element)) {
                    return element;
                }
            }

            return default;
        }
    }
}