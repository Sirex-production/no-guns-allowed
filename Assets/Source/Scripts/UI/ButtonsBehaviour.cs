using MoreMountains.NiceVibrations;
using Support;
using UnityEngine;

namespace Ingame
{
    public class ButtonsBehaviour : MonoBehaviour
    {
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
    }
}