using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class UiCharge : MonoBehaviour
    {
        [SerializeField] private Image chargeImage;
        [Space]
        [SerializeField] private float animationSpeed;

        public bool IsTurnedOn { get; private set; } = true;
        

        private IEnumerator RegenerationAnimationRoutine()
        {
            IsTurnedOn = true;
            
            while (chargeImage.fillAmount <= 1f)
            {
                chargeImage.fillAmount += animationSpeed * Time.deltaTime;
                
                yield return null;
            }

            chargeImage.fillAmount = 1;
        }

        private IEnumerator DisappearanceAnimationRoutine()
        {
            IsTurnedOn = false;
            
            while (chargeImage.fillAmount >= 0)
            {
                chargeImage.fillAmount -= animationSpeed * Time.deltaTime;
                
                yield return null;
            }

            chargeImage.fillAmount = 0;
        }        
        
        public void PlayRegenerateAnimation()
        {
            if(IsTurnedOn)
                return;
            
            StopAllCoroutines();
            StartCoroutine(RegenerationAnimationRoutine());
        }

        public void PlayDisappearanceAnimation()
        {
            if(!IsTurnedOn)
                return;
            
            StopAllCoroutines();
            StartCoroutine(DisappearanceAnimationRoutine());
        }
    }
}