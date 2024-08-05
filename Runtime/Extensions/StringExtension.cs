namespace Ryushin.Extension {
    public static class StringExtension {
        public static bool IsValid(this string str) {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}