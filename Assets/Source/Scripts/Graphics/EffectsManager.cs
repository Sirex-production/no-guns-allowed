using System;
using Support;
using UnityEngine;

namespace Ingame.Graphics
{
    public class EffectsManager : MonoBehaviour
    {
        public event Action OnSlowMotionEnter;
        public event Action OnSlowMotionExit;

        public void EnterSlowMotion()
        {
            OnSlowMotionEnter?.Invoke();
        }

        public void ExitSlowMotion()
        {
            OnSlowMotionExit?.Invoke();
        }
    }
}

