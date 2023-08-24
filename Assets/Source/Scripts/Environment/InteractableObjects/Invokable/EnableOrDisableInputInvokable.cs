using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class EnableOrDisableInputInvokable : MonoInvokable
    {
        [SerializeField] private bool isInputSystemEnabled;

        [Inject] private TouchScreenInputSystem _touchScreenInputSystem;
        
        public override void Invoke()
        {
            _touchScreenInputSystem.Enabled = isInputSystemEnabled;
            
            base.Invoke();
        }
    }
}