using DG.Tweening;
using Extensions;
using MoreMountains.NiceVibrations;
using Support;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class ButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float fadeAnimationDuration = .5f;

        public void OpenUrl(string urlToOpen)
        {
            VibrationController.Vibrate(HapticTypes.Selection);
            
            Application.OpenURL(urlToOpen);
        }

        public void RestartLevel()
        {
            VibrationController.Vibrate(HapticTypes.Selection);
            
            GameController.Instance.RestartLevel();
        }

        public void LoadNextLevel()
        {
            VibrationController.Vibrate(HapticTypes.Selection);
            
            LevelManager.Instance.LoadNextLevel();
        }

        public void ChangeAimSensitivity(Slider slider)
        {
            SaveLoadSystem.Instance.SaveData.AimSensitivity.Value = slider.value;
        }

        public void ClosePauseMenu(CanvasGroup parentCanvasGroup)
        {
            Time.timeScale = 1f;
            
            parentCanvasGroup.SetGameObjectActive();
            parentCanvasGroup.DOFade(0, fadeAnimationDuration)
                .OnComplete(parentCanvasGroup.SetGameObjectInactive);

            SaveLoadSystem.Instance.PerformSave();
        }

        public void OpenPauseMenu(CanvasGroup parentCanvasGroup)
        {
            parentCanvasGroup.SetGameObjectActive();
            parentCanvasGroup.DOFade(1, fadeAnimationDuration)
                .OnComplete(() => Time.timeScale = 0.0001f);
        }
    }
}