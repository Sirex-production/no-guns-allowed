using DG.Tweening;
using Extensions;
using Support;
using Support.SLS;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    public class MainMenuButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float animationDuration = .15f;
        [Inject] private SaveLoadSystem _saveLoadSystem;
        [Inject] private GameController _gameController;
        
        public void ShowMenu(CanvasGroup settingsMenuCanvas)
        {
            //Squeeze animation
            // var initialScale = settingsMenuCanvas.transform.localScale;
            // settingsMenuCanvas.SetGameObjectActive();
            // settingsMenuCanvas.transform.localScale = new Vector3(initialScale.x, 0, initialScale.z);
            // settingsMenuCanvas.transform.DOScaleY(initialScale.y, animationDuration);

            //Fade animation
            settingsMenuCanvas.SetGameObjectActive();
            settingsMenuCanvas.alpha = 0;
            settingsMenuCanvas.DOFade(1, animationDuration);
        }

        public void HideMenu(CanvasGroup settingsMenuCanvas)
        {
            //Squeeze animation
            // var initialScale = settingsMenuCanvas.transform.localScale;
            // Time.timeScale = 1f;
            // settingsMenuCanvas.SetGameObjectActive();
            // settingsMenuCanvas.transform.DOScaleY(0, animationDuration).OnComplete(()=>
            // {
            //     settingsMenuCanvas.SetGameObjectInactive();
            //     settingsMenuCanvas.transform.localScale = initialScale;
            // });
         
            //Fade animation
            settingsMenuCanvas.DOFade(0, animationDuration)
                .OnComplete(settingsMenuCanvas.SetGameObjectInactive);
        }

        public void SaveData()
        {
            _saveLoadSystem.PerformSave();
        }

        public void PlayDevelopersAnimation(UiDevelopers developersMenu)
        {
            developersMenu.HideContent();
            this.WaitAndDoCoroutine(animationDuration, developersMenu.PlayAppearanceAnimation);
        }

        public void LoadNextLevel()
        { 
            _gameController.LoadNextLevel();
        }

        public void LoadLevel(int levelNumber)
        {
            _gameController.LoadLevel(levelNumber);
        }

        public void LoadLastLevelFromSave()
        {
            _gameController.LoadLastLevelFromSave();
        }
    }
}