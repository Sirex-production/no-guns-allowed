using System;
using System.Collections;
using Ingame;
using Support;
using UnityEngine;

namespace Ingame.Graphics
{
    public class SlowMotionEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float timeScale;

        private float _defaultFixedDeltaTime;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Start()
        {
            InputSystem.Instance.OnDragAction += SlowDownTime;
            InputSystem.Instance.OnReleaseAction += ResetTime;
        }

        private void OnDestroy()
        {
            InputSystem.Instance.OnDragAction -= SlowDownTime;
            InputSystem.Instance.OnReleaseAction -= ResetTime;
        }

        private void SlowDownTime(Vector2 _)
        {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
        }

        private void ResetTime(Vector2 _)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = _defaultFixedDeltaTime;
        }
    }
}