using System.Collections.Generic;
using NaughtyAttributes;
using Support;
using Support.SLS;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;

namespace Ingame
{
    public class AnalyticsWrapper : MonoBehaviour
    {
        [SerializeField] private bool isWorking = true;
        [ShowIf("isWorking")]
        [SerializeField] private bool isAnalyticsSentFromEditor = false;
        
        [Inject]
        private GameController _gameController;
        [Inject] 
        private SaveLoadSystem _saveLoadSystem;
        [Inject]
        private TemplateManager _templateManager;

        private LevelStats _levelStats = new LevelStats();

        public LevelStats LevelStats => _levelStats;
        
        protected void Awake()
        {
            Analytics.enabled = isWorking;
        }

        private void Start()
        {
            _levelStats.StartLevel();
            
            _gameController.OnLevelEnded += OnLevelEnded;
            _gameController.OnLevelRestart += OnLevelRestart;
        }

        private void OnDestroy()
        {
            _gameController.OnLevelEnded -= OnLevelEnded;
            _gameController.OnLevelRestart -= OnLevelRestart;
        }

        private void OnLevelRestart()
        {
            _levelStats.StartLevel();
        }

        private void OnLevelEnded(bool isVictory)
        {
            int levelNumber = _saveLoadSystem.SaveData.CurrentLevelNumber.Value;
            
            SendLevelEnd(isVictory, levelNumber);
            SendAverageLevelFps(_templateManager.AverageFPS, levelNumber);
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