using System.Collections;
using DG.Tweening;
using Extensions;
using Support;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class FeedbackScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private CanvasGroup buttonsParentCanvas;
    [SerializeField] [Min(0)] private float spawnDelayTime = 1.1f;
    [SerializeField] [Min(0)] private float letterSpawnPeriod = .05f;
    [SerializeField] [Min(0)] private float fadeAnimationSpeed = 1f;
    [SerializeField] [Min(0)] private float pauseBeforeButtonsWillBeShown = .5f;

    private char[] _letters;
    private string _text;
    private Tween _buttonFadeTween;
    private WaitForSeconds _waitSpawnDelayTime;

    private void Awake()
    {
        _letters = displayedText.text.ToCharArray();
        _text = displayedText.text;
        _waitSpawnDelayTime = new WaitForSeconds(letterSpawnPeriod);
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

    private IEnumerator SpawnTextRoutine()
    {
        displayedText.text = "";
        yield return new WaitForSeconds(spawnDelayTime);

        bool isTag = false;
        var tag = "";
        
        foreach (var letter in _letters)
        {
            if (letter == '<')
                isTag = true;
            else if (letter == '>')
            {
                isTag = false;
                displayedText.text += tag;
                tag = "";
            }

            if (isTag)
            {
                tag += letter;
                continue;
            }
            
            displayedText.text += letter;
            yield return _waitSpawnDelayTime;
        }

        RevealButtons();
    }

    private void SpawnText()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnTextRoutine());
    }

    private void SkipTextRevealEffect(Vector2 _)
    {
        StopAllCoroutines();
        displayedText.text = _text;
        this.WaitAndDo(pauseBeforeButtonsWillBeShown, RevealButtons);
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
