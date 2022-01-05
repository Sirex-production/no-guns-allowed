using System.Collections.Generic;
using NaughtyAttributes;
using Support;
using Support.SLS;
using UnityEngine;
using UnityEngine.Analytics;

namespace Ingame
{
    public class AnalyticsWrapper : MonoSingleton<AnalyticsWrapper>
    {
        [SerializeField] private bool isWorking = true;
        [ShowIf("isWorking")]
        [SerializeField] private bool isAnalyticsSentFromEditor = false;

        private LevelStats _levelStats = new LevelStats();

        public LevelStats LevelStats => _levelStats;
        
        protected override void Awake()
        {
            base.Awake();
            
            Analytics.enabled = isWorking;
        }

        private void Start()
        {
            _levelStats.StartLevel();
            
            GameController.Instance.OnLevelEnded += OnLevelEnded;
            GameController.Instance.OnLevelRestart += OnLevelRestart;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnLevelEnded -= OnLevelEnded;
            GameController.Instance.OnLevelRestart -= OnLevelRestart;
        }

        private void OnLevelRestart()
        {
            _levelStats.StartLevel();
        }

        private void OnLevelEnded(bool isVictory)
        {
            int levelNumber = SaveLoadSystem.Instance.SaveData.CurrentLevelNumber.Value;
            
            SendLevelEnd(isVictory, levelNumber);
            SendAverageLevelFps(TemplateManager.Instance.AverageFPS, levelNumber);
        }
        
        private void SendLevelEnd(bool isVictory, int levelNumber)
        {
            if(Application.isEditor && !isAnalyticsSentFromEditor)
                return;
            
            var eventData = new Dictionary<string, object>()
            {
                {"GameVersion", Application.version},
                {"WasSentFromTheEditor", Application.isEditor},
                {"IsLevelCleared", isVictory},
                {"LevelNumber", levelNumber}
            };

            Analytics.CustomEvent("LevelEnd", eventData);
        }

        private void SendAverageLevelFps(float averageFps, int levelNumber)
        {
            if(Application.isEditor && !isAnalyticsSentFromEditor)
                return;
            
            var eventData = new Dictionary<string, object>()
            {
                {"GameVersion", Application.version},
                {"WasSentFromTheEditor", Application.isEditor},
                {"DeviceModel", SystemInfo.deviceModel},
                {"AverageFPS", averageFps},
                {"LevelNumber", levelNumber}
            };

            Analytics.CustomEvent("AverageFPS", eventData);
        }
    }
}