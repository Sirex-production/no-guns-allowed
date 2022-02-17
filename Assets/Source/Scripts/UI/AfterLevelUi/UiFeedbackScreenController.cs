using DG.Tweening;
using Extensions;
using Support;
using TMPro;
using UnityEngine;

public class UiFeedbackScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private CanvasGroup buttonsParentCanvas;
    [SerializeField] [Min(0)] private float letterSpawnPeriod = .05f;
    [SerializeField] [Min(0)] private float fadeAnimationSpeed = 1f;
    [SerializeField] [Min(0)] private float pauseBeforeButtonsWillBeShown = .5f;

    private bool _isSkipped = false;
    private string _text;
    private Tween _buttonFadeTween;

    private void Awake()
    {
        _text = displayedText.text;
    }

    private void Start()
    {
        InputSystem.Instance.OnTouchAction += SkipTextRevealEffect;
    }

    private void OnDestroy()
    {
        InputSystem.Instance.OnTouchAction -= SkipTextRevealEffect;
        
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
