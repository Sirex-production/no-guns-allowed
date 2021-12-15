using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class UiDevelopers : MonoBehaviour
    {
        [BoxGroup("References")] [Required] [SerializeField] private TMP_Text sirexProductionText;
        [BoxGroup("References")] [Required] [SerializeField] private TMP_Text developersText;
        [BoxGroup("References")] [Required] [SerializeField] private CanvasGroup continueButtonCanvasGroup;

        [BoxGroup("Animation properties")] [SerializeField] [Min(0)] private float lettersSpawnDelayTime = .01f;
        [BoxGroup("Animation properties")] [SerializeField] [Min(0)] private float continueButtonFadeTime = .5f;

        private string _sirexProductionContent;
        private string _developersContent;
        
        public void HideContent()
        {
            _sirexProductionContent = sirexProductionText.text;
            _developersContent = developersText.text;

            sirexProductionText.SetText("");
            developersText.SetText("");
            continueButtonCanvasGroup.alpha = 0;
            continueButtonCanvasGroup.SetGameObjectInactive();
        }

        public void PlayAppearanceAnimation()
        {
            this.SpawnTextCoroutine(sirexProductionText, _sirexProductionContent, lettersSpawnDelayTime, 
                () => this.SpawnTextCoroutine(developersText, _developersContent, lettersSpawnDelayTime, () =>
                    {
                        continueButtonCanvasGroup.SetGameObjectActive();
                        continueButtonCanvasGroup.DOFade(1, continueButtonFadeTime);
                    }));
        }
    }
}