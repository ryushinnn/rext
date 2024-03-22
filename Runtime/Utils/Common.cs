namespace Assassin.Utils {
    public static class Common {
        public static string GetFormatedNumber(float num) {
            return num switch {
                >= 1000000000 => (num / 1000000000).ToString("0.##B"),
                >= 1000000 => (num / 1000000).ToString("0.##M"),
                >= 1000 => (num / 1000).ToString("0.##K"),
                _ => (num / 1000000000).ToString("0.##"),
            };
        }
    }
}