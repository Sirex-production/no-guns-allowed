using DG.Tweening;
using Extensions;
using Support.SLS;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public class MainMenuButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float fadeAnimationTime = .2f;
        [Inject] private SaveLoadSystem _saveLoadSystem;
        
        public void ShowMenu(CanvasGroup settingsMenuCanvas)
        {
            settingsMenuCanvas.SetGameObjectActive();
            settingsMenuCanvas.alpha = 0;
            settingsMenuCanvas.DOFade(1, fadeAnimationTime);
        }

        public void HideMenu(CanvasGroup settingsMenuCanvas)
        {
            settingsMenuCanvas.DOFade(0, fadeAnimationTime)
                .OnComplete(settingsMenuCanvas.SetGameObjectInactive);
        }
        
        public void SaveData()
        {
            _saveLoadSystem.PerformSave();
        }

        public void PlayDevelopersAnimation(UiDevelopers developersMenu)
        {
            developersMenu.HideContent();
            this.WaitAndDoCoroutine(fadeAnimationTime, developersMenu.PlayAppearanceAnimation);
        }
    }
}