namespace RExt.Extensions {
    public static class FloatExtensions {
        public static int ToMilliseconds(this float s) {
            return (int)(s * 1000);
        }
        
        public static float ToSeconds(this int ms) {
            return (float)ms / 1000;
        }
    }
}