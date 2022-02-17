using System;
using Support;

namespace Ingame.Graphics
{
    public class EffectsManager : MonoSingleton<EffectsManager>
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

