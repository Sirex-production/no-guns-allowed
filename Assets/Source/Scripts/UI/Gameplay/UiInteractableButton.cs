using System;
using Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class UiInteractableButton : MonoBehaviour
    {
        [BoxGroup("References")]
        [Required][SerializeField] private UnityEngine.UI.Button interactableButton;
        [Space]
        [BoxGroup("References")]
        [Required] [SerializeField] private Image interactableButtonImageBackground;
        [BoxGroup("References")]
        [Required][SerializeField] private TMP_Text interactableButtonText;
        
        [BoxGroup("Animation options")] 
        [SerializeField] [Min(0)] private float animationSpeed = 1;

        private void Start()
        {
            interactableButton.SetGameObjectInactive();
            UiController.Instance.OnInteractableButtonShown += ShowInteractableButton;
            UiController.Instance.OnInteractableButtonHidden += HideInteractableButton;
        }

        private void OnDestroy()
        {
            UiController.Instance.OnInteractableButtonShown -= ShowInteractableButton;
            UiController.Instance.OnInteractableButtonHidden -= HideInteractableButton;
        }

        private void ShowInteractableButton(Action onButtonPressed)
        {
            interactableButton.SetGameObjectActive();
            interactableButtonImageBackground.SetGameObjectActive();
            interactableButtonImageBackground.fillAmount = 0;
            this.LerpCoroutine(animationSpeed, 0, 1,
                f => interactableButtonImageBackground.fillAmount = f,
                () => interactableButtonText.SetGameObjectActive());
            
            interactableButton.onClick.AddListener(new UnityAction(onButtonPressed));
            interactableButton.onClick.AddListener(HideInteractableButton);
        }

        private void HideInteractableButton()
        {
            interactableButton.onClick.RemoveAllListeners();
            interactableButton.SetGameObjectInactive();
        }
    }
}