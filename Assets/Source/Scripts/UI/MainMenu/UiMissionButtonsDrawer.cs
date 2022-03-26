using System;
using System.Collections.Generic;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class UiMissionButtonsDrawer : MonoBehaviour
    {
        [Tooltip("Defines whether mission buttons will be shown due to players progress or always available")]
        [SerializeField] private bool areButtonsUpdatedDueToProgress = true;
        [SerializeField] private List<Button> levelButtons;

        [Inject] private SaveLoadSystem _saveLoadSystem;

        private void Awake()
        {
            if(areButtonsUpdatedDueToProgress)
                SetAvailabilityOfLevelsButtonsDueToProgress();
        }

        private void SetAvailabilityOfLevelsButtonsDueToProgress()
        {
            var lastAvailableLevelIndex = _saveLoadSystem.SaveData.LastUnlockedLevelNumber.Value;

            for (var i = 0; i < levelButtons.Count; i++)
            {
                var levelButton = levelButtons[i];
                if (levelButton == null)
                    throw new NullReferenceException("Level button can not be null");
                
                levelButton.interactable = i < lastAvailableLevelIndex;
            }
        }
    }
}