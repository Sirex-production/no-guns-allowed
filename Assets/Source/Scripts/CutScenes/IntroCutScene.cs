using Ingame.UI;
using NaughtyAttributes;
using Support;
using Support.Sound;
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
    [BoxGroup("Level settings")]
    [SerializeField] private int levelToLoadAfterTheDialog;
    
    [Inject] private readonly GameController _gameController;
    [Inject] private readonly UiNarrative _uiNarrative;

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
        _uiNarrative.PlayDialog(dialogData, () => _gameController.LoadLevel(levelToLoadAfterTheDialog));
    }
}
