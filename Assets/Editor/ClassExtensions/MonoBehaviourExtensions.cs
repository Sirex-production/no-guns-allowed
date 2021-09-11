using System;
using System.Collections;
using UnityEngine;

namespace Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void DoAfterNextFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            monoBehaviour.StartCoroutine(WaitOneFrameAndDoRoutine(action));
        }

        private static IEnumerator WaitOneFrameAndDoRoutine(Action action)
        {
            yield return null;
            
            action?.Invoke();
        }
    }
}