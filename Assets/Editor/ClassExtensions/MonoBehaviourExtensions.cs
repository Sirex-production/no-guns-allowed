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

        public static void WaitAndDo(this MonoBehaviour monoBehaviour, float timeToWait, Action action)
        {
            monoBehaviour.StartCoroutine(WaitAndDoRoutine(timeToWait, action));
        }

        private static IEnumerator WaitOneFrameAndDoRoutine(Action action)
        {
            yield return null;
            
            action?.Invoke();
        }

        private static IEnumerator WaitAndDoRoutine(float timeToWait, Action action)
        {
            yield return new WaitForSeconds(timeToWait);
            
            action?.Invoke();
        }
    }
}