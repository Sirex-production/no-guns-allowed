using MoreMountains.NiceVibrations;
using Support;
using UnityEngine;

namespace Ingame
{
    public class ButtonsBehaviour : MonoBehaviour
    {
        [Header("OpenUrl parameters")]
        [SerializeField] private string urlToOpen = "https://discord.gg/p8rmcv2dJk";
        
        public void OpenUrl()
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