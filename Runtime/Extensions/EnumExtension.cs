using System;
using System.ComponentModel;

namespace Ryushin.Extension {
    [Flags]
    public enum FlagEnumExample {
        None = 0,
        First = 0x01,
        Second = 0x02,
        Third = 0x04,
        Fourth = 0x08,
        Fifth = 0x10,
        Sixth = 0x20,
        FirstAndSecond = First | Second,
        ExceptFirst = Second | Third | Fourth | Fifth | Sixth,
        // ...
        All = First | Second | Third | Fourth | Fifth | Sixth
    }
    
    public static class EnumExtension {
        public static string ToDescriptionString(this Enum value) {
            var attributes = (DescriptionAttribute[]) value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionLowerString(this Enum value) {
            var attributes = (DescriptionAttribute[]) value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description.ToLower() : string.Empty;
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum {
            foreach (var field in typeof(T).GetFields()) {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) {
                    if (attribute.Description == description) return (T) field.GetValue(null);
                } else {
                    if (field.Name == description) return (T) field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static int IntValue(this Enum value) {
            return Convert.ToInt32(value);
        }

        public static T Next<T>(this T source) where T : struct {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var arr = (T[]) Enum.GetValues(source.GetType());
            var j = Array.IndexOf(arr, source) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }

        /// <summary>
        /// Check if an enum (combination) contains another enum
        /// </summary>
        public static bool Has(this Enum combination, Enum value) {
            if (combination.GetType() != value.GetType()) throw new ArgumentException("Type mismatch");
            return (combination.IntValue() & value.IntValue()) == value.IntValue();
        }
    }
}