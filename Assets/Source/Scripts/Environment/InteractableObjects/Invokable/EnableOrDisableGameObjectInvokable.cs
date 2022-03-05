using UnityEngine;

namespace Ingame
{
    public class EnableOrDisableGameObjectInvokable : MonoInvokable
    {
        [SerializeField] private bool enabled;
        [SerializeField] private GameObject gameObject;
        
        public override void Invoke()
        {
            gameObject.SetActive(enabled);
            
            base.Invoke();
        }
    }
}