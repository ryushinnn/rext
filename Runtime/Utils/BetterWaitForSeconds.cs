using System.Collections.Generic;
using UnityEngine;

namespace RExt.Utils {
    /// <summary>
    /// How to use: yield return BetterWaitForSeconds.Wait(1f) or yield return BetterWaitForSeconds.WaitRealtime(1f);
    /// </summary>
    public static class BetterWaitForSeconds {
        class WaitForSeconds : CustomYieldInstruction {
            float waitUntil;

            public override bool keepWaiting {
                get {
                    if (Time.time < waitUntil)
                        return true;

                    Pool(this);
                    return false;
                }
            }

            public void Initialize(float seconds) {
                waitUntil = Time.time + seconds;
            }
        }

        class WaitForSecondsRealtime : CustomYieldInstruction {
            float waitUntil;

            public override bool keepWaiting {
                get {
                    if (Time.realtimeSinceStartup < waitUntil)
                        return true;

                    Pool(this);
                    return false;
                }
            }

            public void Initialize(float seconds) {
                waitUntil = Time.realtimeSinceStartup + seconds;
            }
        }

        const int POOL_INITIAL_SIZE = 4;

        static readonly Stack<WaitForSeconds> WaitForSecondsPool;
        static readonly Stack<WaitForSecondsRealtime> WaitForSecondsRealtimePool;

        static BetterWaitForSeconds() {
            WaitForSecondsPool = new Stack<WaitForSeconds>(POOL_INITIAL_SIZE);
            WaitForSecondsRealtimePool = new Stack<WaitForSecondsRealtime>(POOL_INITIAL_SIZE);

            for (int i = 0; i < POOL_INITIAL_SIZE; i++) {
                WaitForSecondsPool.Push(new WaitForSeconds());
                WaitForSecondsRealtimePool.Push(new WaitForSecondsRealtime());
            }
        }

        public static CustomYieldInstruction Wait(float seconds) {
            WaitForSeconds instance;
            if (WaitForSecondsPool.Count > 0)
                instance = WaitForSecondsPool.Pop();
            else
                instance = new WaitForSeconds();

            instance.Initialize(seconds);
            return instance;
        }

        public static CustomYieldInstruction WaitRealtime(float seconds) {
            WaitForSecondsRealtime instance;
            if (WaitForSecondsRealtimePool.Count > 0)
                instance = WaitForSecondsRealtimePool.Pop();
            else
                instance = new WaitForSecondsRealtime();

            instance.Initialize(seconds);
            return instance;
        }

        static void Pool(WaitForSeconds instance) {
            WaitForSecondsPool.Push(instance);
        }

        static void Pool(WaitForSecondsRealtime instance) {
            WaitForSecondsRealtimePool.Push(instance);
        }
    }
}