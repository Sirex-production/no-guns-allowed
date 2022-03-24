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
    }
    
    private void ShowHeader()
    {
        _uiNarrative.ShowNarrativeSection();
        _uiNarrative.PrintHeaderText(headerText);
    }

    private void ShowSubHeader()
    {
        _uiNarrative.PrintSubHeaderText(subheaderText);
    }

    private void HideHeaderAndSubHeader()
    {
        _uiNarrative.HideHeaderText();
        _uiNarrative.HideSubHeaderText();
    }

    private void PlayDialog()
    {
        _uiNarrative.PlayDialog(dialogData, EndCutScene);
    }

    private void EndCutScene()
    {
        _uiNarrative.HideNarrativeSection();

        _gameController.EndCutScene();
    }
}
