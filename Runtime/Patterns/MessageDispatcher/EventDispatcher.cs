using System;

namespace RExt.Patterns.EventDispatcher {
    public class EventDispatcher<T> where T : Delegate {
        /// <summary>
        /// Use this to trigger messages
        /// </summary>
        public static T Trigger { get; private set; }

        /// <summary>
        /// Register callback to a message
        /// </summary>
        /// <param name="callback">function that will be executed when the message is triggered</param>
        public static void AddListener(T callback) {
            Trigger = (T)Delegate.Combine(Trigger, callback);
        }

        /// <summary>
        /// Unregister callback from a message
        /// </summary>
        /// <param name="callback">function that is no longer executed when the message is triggered</param>
        public static void RemoveListener(T callback) {
            Trigger = (T)Delegate.Remove(Trigger, callback);
        }
    }
    
    public class EventExample {
        public delegate void Example1();
        public delegate void Example2(int value);
        public delegate void Example3(string value1, float value2);
        // ...
    }
}