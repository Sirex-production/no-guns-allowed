using System;
using NaughtyAttributes;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class UiController : MonoBehaviour
    {
        [Required] 
        [SerializeField] private UiPlayerDashesController uiDashesController;
        [SerializeField] private Toggle[] uiVibrationOptionToggles;

        [Inject] private SaveLoadSystem _saveLoadSystem;
        
        public event Action<string, LogDisplayType> OnLogMessageDisplayed;
        public event Action<Action> OnInteractableButtonShown;
        public event Action OnInteractableButtonHidden;

        public UiPlayerDashesController UiDashesController => uiDashesController;

        private void Start()
        {
            SetValuesToVibrationToggles();
        }

        private void SetValuesToVibrationToggles()
        {
            var saveData = _saveLoadSystem.SaveData;
            
            foreach (var toggle in uiVibrationOptionToggles)
            {
                if(toggle == null)
                    continue;
                
                toggle.isOn = saveData.IsVibrationEnabled.Value;
            }
        }

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