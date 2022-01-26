using DG.Tweening;
using Extensions;
using MoreMountains.NiceVibrations;
using Support;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame
{
    public class TemplateButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float fadeAnimationDuration = .5f;
        [Inject] 
        private GameController _gameController;
        [Inject]
        private SaveLoadSystem _saveLoadSystem;
        
        
        public void OpenUrl(string urlToOpen)
        {
            Application.OpenURL(urlToOpen);
        }

        public void RestartLevel()
        {
            _gameController.RestartLevel();
        }

        public void LoadNextLevel()
        {
            _gameController.LoadNextLevel();
        }

        public void ClickVibrate()
        {
            VibrationController.Vibrate(HapticTypes.Selection);
        }

        public void ChangeAimSensitivity(Slider slider)
        {
            _saveLoadSystem.SaveData.AimSensitivity.Value = slider.value;
        }

        public void ClosePauseMenu(CanvasGroup parentCanvasGroup)
        {
            Time.timeScale = 1f;
            
            parentCanvasGroup.SetGameObjectActive();
            parentCanvasGroup.DOFade(0, fadeAnimationDuration)
                .OnComplete(parentCanvasGroup.SetGameObjectInactive);

            _saveLoadSystem.PerformSave();
        }

        public void OpenPauseMenu(CanvasGroup parentCanvasGroup)
        {
            parentCanvasGroup.SetGameObjectActive();
            parentCanvasGroup.DOFade(1, fadeAnimationDuration)
                .OnComplete(() => Time.timeScale = 0.0001f);
        }

        public void TransitBetweenLevelOverviews()
        {
            var sectionManager = SectionsManager.Instance;
            
            if(sectionManager == null)
                return;
            
            if(sectionManager.IsInLevelOverview)
                sectionManager.ExitLevelOverview();
            else
                sectionManager.EnterLevelOverview();
        }
    }
}