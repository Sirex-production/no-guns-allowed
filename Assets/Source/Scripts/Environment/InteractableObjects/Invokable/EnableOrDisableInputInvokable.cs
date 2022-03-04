using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class EnableOrDisableInputInvokable : MonoInvokable
    {
        [SerializeField] private bool isInputSystemEnabled;

        [Inject] private InputSystem _inputSystem;
        
        public override void Invoke()
        {
            _inputSystem.Enabled = isInputSystemEnabled;
            
            base.Invoke();
        }
    }
}