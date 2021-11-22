using Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.UI
{
    public class UiBeforeLevel : MonoBehaviour
    {
        [Required] [SerializeField] private Animator enterExitLevelAnimator;

        public void PlayCloseGateAnimation()
        {
            if(enterExitLevelAnimator == null)
                return;
            
            enterExitLevelAnimator.SetGameObjectActive();
            enterExitLevelAnimator.SetTrigger("CloseGate");
        }
    }
}