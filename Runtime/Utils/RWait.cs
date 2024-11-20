using System.Collections.Generic;
using UnityEngine;

namespace RExt.Utils {
    public static class RWait {
        static Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static WaitForSeconds GetWait(float time) {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        static Dictionary<float, WaitForSecondsRealtime> WaitRealTimeDictionary = new();

        public static WaitForSecondsRealtime GetWaitRealTime(float time) {
            if (WaitRealTimeDictionary.TryGetValue(time, out var wait)) return wait;
            WaitRealTimeDictionary[time] = new WaitForSecondsRealtime(time);
            return WaitRealTimeDictionary[time];
        }
    }
}