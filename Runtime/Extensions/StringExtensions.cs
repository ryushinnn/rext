namespace RExt.Extensions {
    public static class StringExtensions {
        public static bool IsValid(this string str) {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}