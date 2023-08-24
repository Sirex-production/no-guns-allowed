using DG.Tweening;
using Support;
using Support.Extensions;
using TMPro;
using UnityEngine;
using Zenject;

public class UiFeedbackScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private CanvasGroup buttonsParentCanvas;
    [SerializeField] [Min(0)] private float letterSpawnPeriod = .05f;
    [SerializeField] [Min(0)] private float fadeAnimationSpeed = 1f;
    [SerializeField] [Min(0)] private float pauseBeforeButtonsWillBeShown = .5f;

    [Inject] private TouchScreenInputSystem _touchScreenInputSystem;
    
    private bool _isSkipped = false;
    private string _text;
    private Tween _buttonFadeTween;

    private void Awake()
    {
        _text = displayedText.text;
    }

    private void Start()
    {
        _touchScreenInputSystem.OnTouchAction += SkipTextRevealEffect;
    }

    private void OnDestroy()
    {
        _touchScreenInputSystem.OnTouchAction -= SkipTextRevealEffect;
        
        if(_buttonFadeTween != null)
            _buttonFadeTween.Kill();
    }

    private void OnEnable()
    {
        SpawnText();
        HideButtons();
    }

    private void SpawnText()
    {
        StopAllCoroutines();
        this.SpawnTextCoroutine(displayedText, _text, letterSpawnPeriod, RevealButtons);
    }

    private void SkipTextRevealEffect(Vector2 _)
    {
        if(_isSkipped)
            return;
        _isSkipped = true;
        
        StopAllCoroutines();
        displayedText.text = _text;
        this.WaitAndDoCoroutine(pauseBeforeButtonsWillBeShown, RevealButtons);
    }

    private void RevealButtons()
    {
        if(_buttonFadeTween != null)
            _buttonFadeTween.Kill();

        buttonsParentCanvas.SetGameObjectActive();
        
        buttonsParentCanvas.alpha = 0;
        _buttonFadeTween = buttonsParentCanvas.DOFade(1, fadeAnimationSpeed);
    }

    private void HideButtons()
    {
        if(_buttonFadeTween != null)
            _buttonFadeTween.Kill();
        
        buttonsParentCanvas.SetGameObjectInactive();
    }
}
