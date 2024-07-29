using System;
using System.Collections.Generic;

namespace Assassin.Extension {
    public static class ArrayExtension {
        public static T Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find(array, match);
        }
    }
}