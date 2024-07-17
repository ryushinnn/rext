using System;

namespace Assassin.Utils {
    public class MessageDispatcher<T> where T : Delegate {
        /// <summary>
        /// Use this to trigger messages
        /// </summary>
        public static T Trigger { get { return _handle; } }
        private static T _handle;

        /// <summary>
        /// Register callback to a message
        /// </summary>
        /// <param name="callback">function that will be executed when the message is triggered</param>
        public static void AddListener(T callback) {
            _handle = (T)Delegate.Combine(_handle, callback);
        }

        /// <summary>
        /// Unregister callback from a message
        /// </summary>
        /// <param name="callback">function that is no longer executed when the message is triggered</param>
        public static void RemoveListener(T callback) {
            _handle = (T)Delegate.Remove(_handle, callback);
        }
    }
}