using System;
using Extensions;
using Ingame.UI;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

public class IntroCutScene : MonoBehaviour
{
    [BoxGroup("Narrative settings")] 
    [SerializeField] private DialogData dialogData;
    [BoxGroup("Narrative settings")] 
    [SerializeField] private string headerText;
    [BoxGroup("Narrative settings")] 
    [SerializeField] private string subheaderText;
    
    [Inject] private GameController _gameController;
    [Inject] private UiNarrative _uiNarrative;

    private void Start()
    {
        _gameController.OnGameplayStarted += OnGameplayStarted;
    }

    private void OnDestroy()
    {
        _gameController.OnGameplayStarted -= OnGameplayStarted;
    }

    private void OnGameplayStarted()
    {
        this.SetGameObjectInactive();
    }

    private void StartCutScene()
    {
        _gameController.StartCutScene();
        
        //todo remove hardcode
        this.WaitAndDoCoroutine(1f, () => _uiNarrative.ShowNarrativeSection());
        this.WaitAndDoCoroutine(2f, () => _uiNarrative.PrintHeaderText(headerText));
        this.WaitAndDoCoroutine(3f, () => _uiNarrative.PrintSubHeaderText(subheaderText));
        this.WaitAndDoCoroutine(6f,
            () =>
            {
                _uiNarrative.HideHeaderText();
                _uiNarrative.HideSubHeaderText();
                _uiNarrative.PlayDialog(dialogData);
            });
    }

    private void EndCutScene()
    {
        _uiNarrative.HideNarrativeSection();

        _gameController.EndCutScene();
    }
}
