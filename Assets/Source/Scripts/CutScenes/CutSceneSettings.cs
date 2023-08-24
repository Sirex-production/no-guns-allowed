using Support.Extensions;
using Ingame.UI;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class CutSceneSettings : MonoBehaviour
    {
        [BoxGroup("Cutscene skip properties")]
        [SerializeField] private bool allowSkipButton = true;
        [BoxGroup("Cutscene skip properties")]
        [SerializeField] [Min(0) ] private float delayAfterSkipButtonHides = 1f;

        [Inject] private readonly TouchScreenInputSystem _touchScreenInputSystem;
        [Inject] private readonly UiNarrative _uiNarrative;

        private void Start()
        {
            _touchScreenInputSystem.OnTouchAction += OnTouchAction;
        }

        private void OnDestroy()
        {
            _touchScreenInputSystem.OnTouchAction -= OnTouchAction;
        }
        
        private void OnTouchAction(Vector2 _)
        {
            if(!allowSkipButton)
                return;
            
            _uiNarrative.ShowSkipButton();
            
            StopAllCoroutines();
            this.WaitAndDoCoroutine(delayAfterSkipButtonHides, _uiNarrative.HideSkipButton);
        }
    }
}