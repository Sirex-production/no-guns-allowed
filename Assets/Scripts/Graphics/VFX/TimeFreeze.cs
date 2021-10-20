using System.Collections;
using Ingame;
using UnityEngine;

namespace Ingame.Graphics
{
    public class TimeFreeze : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 0.2f)] private float timeScale;
        [SerializeField] [Min(0.0f)] private float freezeDuration;

        private float _defaultFixedDeltaTime;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private IEnumerator TimeFreezeRoutine()
        {
            Time.timeScale = timeScale;
            //Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;

            yield return new WaitForSeconds(freezeDuration * timeScale);

            //Time.fixedDeltaTime = _defaultFixedDeltaTime;
            Time.timeScale = 1.0f;
        }

        private void FreezeTime(Vector3 _)
        {
            StopAllCoroutines();
            StartCoroutine(TimeFreezeRoutine());
        }
    }
}