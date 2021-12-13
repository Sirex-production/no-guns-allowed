using Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Ingame.UI
{
    public class UiPlayerLog : MonoBehaviour
    {
        [Required]
        [SerializeField] private TMP_Text logTextArea;
        [SerializeField] [Min(0)] private float letterSpawnDelayTime = .01f;
        [Tooltip("Pause after Log will be cleared if temporary log was sent")]
        [SerializeField] [Min(0)] private float pauseAfterTmpLogWillBeCleared = 2f;

        private string _initialLogContent;

        private void Awake()
        {
            _initialLogContent = logTextArea.text;
        }

        private void Start()
        {
            UiController.Instance.OnLogMessageDisplayed += SetLogContent;
            
            if(PlayerEventController.Instance == null)
                return;

            PlayerEventController.Instance.OnDashCancelled += OnDashCanceled;
        }

        private void OnDestroy()
        {
            UiController.Instance.OnLogMessageDisplayed -= SetLogContent;
            
            if(PlayerEventController.Instance == null)
                return;

            PlayerEventController.Instance.OnDashCancelled -= OnDashCanceled;
        }

        private void OnDashCanceled()
        {
            SetLogContent("Out of charges", LogDisplayType.DisplayAndClear);
        }

        private void SetLogContent(string message, LogDisplayType logDisplayType)
        {
            message = $"{_initialLogContent}{message}";
            
            StopAllCoroutines();
            logTextArea.SetText("");
            this.SpawnTextCoroutine(logTextArea, message, letterSpawnDelayTime);

            switch (logDisplayType)
            {
                case LogDisplayType.DisplayAndClear:
                    this.WaitAndDoCoroutine(pauseAfterTmpLogWillBeCleared, () => SetLogContent("...", LogDisplayType.DisplayAndKeep));
                    break;
                case LogDisplayType.DisplayAndKeep:
                    break;
            }
        }
    }

    public enum LogDisplayType
    {
        DisplayAndClear,
        DisplayAndKeep
    }
}