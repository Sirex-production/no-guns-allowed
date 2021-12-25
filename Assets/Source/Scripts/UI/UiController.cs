using System;
using NaughtyAttributes;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class UiController : MonoSingleton<UiController>
    {
        [Required] [SerializeField] private UiPlayerDashesController uiDashesController;

        public event Action<string, LogDisplayType> OnLogMessageDisplayed;
        public event Action<Action> OnInteractableButtonShown;
        public event Action OnInteractableButtonHidden;

        public UiPlayerDashesController UiDashesController => uiDashesController;

        public void DisplayLogMessage(string message, LogDisplayType logDisplayType)
        {
            OnLogMessageDisplayed?.Invoke(message, logDisplayType);
        }

        public void ShowInteractableButton(Action onClick)
        {
            OnInteractableButtonShown?.Invoke(onClick);
        }

        public void HideInteractableButton()
        {
            OnInteractableButtonHidden?.Invoke();
        }
    }
}