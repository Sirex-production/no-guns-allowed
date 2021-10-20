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
            Application.OpenURL(urlToOpen);
        }

        public void RestartLevel()
        {
            GameController.Instance.RestartLevel();
        }

        public void LoadNextLevel()
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}