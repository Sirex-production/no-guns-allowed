using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Support.Extensions
{
    /// <summary>
    /// Class that holds all extension methods for MonoBehaviour class 
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        private static IEnumerator WaitAndDoRoutine(float pause, Action action)
        {
            yield return new WaitForSeconds(pause);
            
            action?.Invoke();
        }

        private static IEnumerator DoAfterNextFrameRoutine(Action action)
        {
            yield return null;
            
            action?.Invoke();
        }

        private static IEnumerator RepeatRoutine(float pause, Action action, bool startWithPause)
        {
            var interval = new WaitForSeconds(pause);

            if (startWithPause)
                yield return interval;

            while (true)
            {
                action?.Invoke();
                
                yield return interval;
            }
        }

        private static IEnumerator LerpRoutine(float speed, float a, float b, Action<float> action, Action onComplete)
        {
            if(action == null)
                yield break;
            
            speed = b < a ? -speed : speed;

            float currentValue = a;

            while (Math.Abs(currentValue - b) > .001f)
            {
                action(currentValue);
                currentValue += Time.deltaTime * speed;
                
                yield return null;
            }
            
            action(b);
            onComplete?.Invoke();
        }
        
        private static IEnumerator SpawnTextRoutine(TMP_Text textArea, string textToDisplay, float spawnDelayTime, Action onComplete, Action onLetterSpawned)
        {
            if (String.IsNullOrEmpty(textToDisplay))
            {
                onComplete?.Invoke();
                yield break;
            }

            spawnDelayTime = Mathf.Abs(spawnDelayTime);

            var letters = textToDisplay.ToCharArray();
            var waitForDelayInSeconds = new WaitForSeconds(spawnDelayTime);
            textArea.text = "";

            yield return waitForDelayInSeconds;

            bool isTag = false;
            var tag = "";
            
            foreach (var letter in letters)
            {
                if (letter == '<')
                    isTag = true;
                else if (letter == '>')
                {
                    isTag = false;
                    textArea.SetText(textArea.text + tag);
                    tag = "";
                }

                if (isTag)
                {
                    tag += letter;
                    continue;
                }
            
                textArea.SetText(textArea.text + letter);
                
                onLetterSpawned?.Invoke();
                yield return waitForDelayInSeconds;
            }
            
            onComplete?.Invoke();
        }

        private static IEnumerator DeleteTextLetterByLetterRoutine(TMP_Text textArea, float spawnDelayTime, Action onComplete)
        {
            if (String.IsNullOrEmpty(textArea.text))
            {
                onComplete?.Invoke();
                yield break;
            }

            spawnDelayTime = Mathf.Abs(spawnDelayTime);
            
            var waitForDelayInSeconds = new WaitForSeconds(spawnDelayTime);

            yield return waitForDelayInSeconds;

            while (textArea.text.Length > 0)
            {
                textArea.SetText(textArea.text.Remove(textArea.text.Length - 1, 1));

                yield return waitForDelayInSeconds;
            }
            
            onComplete?.Invoke();
        }

        /// <summary>
        /// Starts coroutine that repeats function with some pause
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="pause">Pause between invoking function</param>
        /// <param name="action">Function that will be invoked</param>
        /// <param name="startWithPause">Defines weather coroutine makes pause before invoking function for the first time</param>
        /// <returns>Returns started coroutine</returns>
        public static Coroutine RepeatCoroutine(this MonoBehaviour monoBehaviour, float pause, Action action, bool startWithPause = false)
        {
            return monoBehaviour.StartCoroutine(RepeatRoutine(pause, action, startWithPause));
        }

        /// <summary>
        /// Invokes method on the next frame
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="action">Function that will be invoked</param>
        /// <returns>Returns started coroutine</returns>
        public static Coroutine DoAfterNextFrameCoroutine(this MonoBehaviour monoBehaviour, Action action)
        {
            return monoBehaviour.StartCoroutine(DoAfterNextFrameRoutine(action));
        }

        /// <summary>
        /// Invokes function after some time
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="pause">Pause after function will be invoked</param>
        /// <param name="action">Function that will be invoked</param>
        /// <returns>Returns started coroutine</returns>
        public static Coroutine WaitAndDoCoroutine(this MonoBehaviour monoBehaviour, float pause, Action action)
        {
            return monoBehaviour.StartCoroutine(WaitAndDoRoutine(pause, action));
        }

        /// <summary>
        /// Lerps between two values with given speed
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="speed">Lerping speed</param>
        /// <param name="a">Initial value</param>
        /// <param name="b">Target value</param>
        /// <param name="action">Function that will be invoked on each frame. Takes float that represents lerping value at a given moment of time</param>
        /// <returns></returns>
        public static Coroutine LerpCoroutine(this MonoBehaviour monoBehaviour, float speed, float a, float b, Action<float> action, Action onComplete = null)
        {
            return monoBehaviour.StartCoroutine(LerpRoutine(speed, a, b, action, onComplete));
        }

        /// <summary>
        /// Display content in certain text area with writing effect
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="textArea">Area where content will be displayed</param>
        /// <param name="textToDisplay">Content that text area will display</param>
        /// <param name="spawnDelayTime">Pause between appearing letters</param>
        /// <param name="onComplete">Action that will be invoked when deletion will be completed</param>
        /// <param name="onLetterSpawned">Action that is invoked when letter is spawned</param>
        /// <returns></returns>
        public static Coroutine SpawnTextCoroutine(this MonoBehaviour monoBehaviour, TMP_Text textArea, string textToDisplay, float spawnDelayTime, Action onComplete = null,  Action onLetterSpawned = null)
        {
            return monoBehaviour.StartCoroutine(SpawnTextRoutine(textArea, textToDisplay, spawnDelayTime, onComplete, onLetterSpawned));
        }

        /// <summary>
        /// Deletes letter by letter from given text area
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="textArea">Area from which content will be deleted</param>
        /// <param name="delayBeforeRemovingLetters">Pause between deleting letters</param>
        /// <param name="onComplete">Action that will be invoked when deletion will be completed</param>
        /// <returns></returns>
        public static Coroutine DeleteTextLetterByLetterCoroutine(this MonoBehaviour monoBehaviour, TMP_Text textArea, float delayBeforeRemovingLetters, Action onComplete = null)
        {
            return monoBehaviour.StartCoroutine(DeleteTextLetterByLetterRoutine(textArea, delayBeforeRemovingLetters, onComplete));
        }
    }
}