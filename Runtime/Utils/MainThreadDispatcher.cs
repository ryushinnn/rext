using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assassin.Utils {
    public class MainThreadDispatcher : MonoBehaviour {
        private static readonly Queue<Action> _executionQueue = new();
        
        /// <summary>
        /// Lock the queue and add the IEnumerator to the queue 
        /// </summary>
        /// <param name="action">function that will be executed from the main thread</param>
        public void Enqueue(IEnumerator action) {
            lock (_executionQueue) {
                _executionQueue.Enqueue(() => { StartCoroutine(action);});
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