using UnityEngine;

namespace Ingame
{
    public abstract class MonoInvokable : MonoBehaviour, IInvokable
    {
        [SerializeField] private bool destroyAfterInvoke = false;

        public virtual void Invoke()
        {
            if(destroyAfterInvoke)
                Destroy(this);
        }
    }
}