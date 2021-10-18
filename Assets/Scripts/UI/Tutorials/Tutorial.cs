using UnityEngine;

namespace Ingame.UI
{
    public abstract class Tutorial : MonoBehaviour
    {
        protected virtual void Complete()
        {
            TutorialsManager.Instance.ActivateNext();
        }

        public abstract void Activate();
    }
}