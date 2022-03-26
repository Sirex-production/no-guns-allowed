using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class DisableButtonInvokable : MonoInvokable
    {
        [Required]
        [SerializeField] private InvokableButton invokableButton;

        public override void Invoke()
        {
            invokableButton.Disable();
            
            base.Invoke();
        }
    }
}