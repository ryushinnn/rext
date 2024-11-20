using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RExt.Utils {
    public class MainThreadDispatcher : MonoBehaviour {
        static Queue<Action> ExecutionQueue = new();
        
        /// <summary>
        /// Lock the queue and add the IEnumerator to the queue 
        /// </summary>
        /// <param name="action">function that will be executed from the main thread</param>
        public void Enqueue(IEnumerator action) {
            lock (ExecutionQueue) {
                ExecutionQueue.Enqueue(() => { StartCoroutine(action);});
            }
        }

        /// <summary>
        /// Lock the queue and add the IEnumerator to the queue 
        /// </summary>
        /// <param name="action">function that will be executed from the main thread</param>
        public void Enqueue(Action action) {
            Enqueue(ActionWrapper(action));
        }

        private IEnumerator ActionWrapper(Action action) {
            action();
            yield return null;
        }
    }   
}