using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Support;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiTextTypingOutEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUIComponent;
    [SerializeField] private Button[] buttons;
    [SerializeField] [ReadOnly] [Min(0)] private float spawnDelayTime;
    [SerializeField] [ReadOnly] [Min(0)] private float letterSpawnPeriod;

    [ReadOnly] private char[] _letters;
    [ReadOnly] private string _text;

    private void Awake()
    {
        _letters = textMeshProUGUIComponent.text.ToCharArray();
        _text = textMeshProUGUIComponent.text;
    }

    private void Start()
    {
        InputSystem.Instance.OnTouchAction += SkipTextRevealEffect;
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnTextRoutine());
        foreach (var button in buttons)
        {
            button.SetGameObjectInactive();
        }
    }

    private void OnDestroy()
    {
        InputSystem.Instance.OnTouchAction -= SkipTextRevealEffect;
    }

    private IEnumerator SpawnTextRoutine()
    {
        textMeshProUGUIComponent.text = "";
        yield return new WaitForSeconds(spawnDelayTime);

        foreach (var t in _letters)
        {
            textMeshProUGUIComponent.text += t;
            yield return new WaitForSeconds(letterSpawnPeriod);
        }

        RevealButtons();
    }

    private void SkipTextRevealEffect(Vector2 _)
    {
        StopAllCoroutines();
        textMeshProUGUIComponent.text = _text;
        RevealButtons();
    }

    private void RevealButtons()
    {
        foreach (var button in buttons)
        {
            button.SetGameObjectActive();
        }
    }
}
