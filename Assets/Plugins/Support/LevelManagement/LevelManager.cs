using System;
using Support.SLS;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Support
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        
        [Inject] private SaveLoadSystem _saveLoadSystem;

        private void UpdateLevelProgress(int levelNumber)
        {
            if (levelNumber <= 0 || _saveLoadSystem.SaveData.CurrentLevelNumber.Value == levelNumber)
                return;

            _saveLoadSystem.SaveData.CurrentLevelNumber.Value = levelNumber;

            if (_saveLoadSystem.SaveData.LastUnlockedLevelNumber.Value < levelNumber)
                _saveLoadSystem.SaveData.LastUnlockedLevelNumber.Value = levelNumber;
            
            _saveLoadSystem.PerformSave();
        }

        public void LoadLevel(int levelNumber)
        {
            if (levelNumber < 0)
                throw new ArgumentException($"There is no level with such index \"{levelNumber}\"");
            
            var sceneIndex = levelData.GetSceneIndexByLevel(levelNumber);
            
            UpdateLevelProgress(levelNumber);
            SceneManager.LoadScene(sceneIndex);
        }
        
        /// <summary>Restarts last level that was saved in progress(SaveLoadSystem)</summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadLastPlayedLevel()
        {
            LoadLevel(_saveLoadSystem.SaveData.CurrentLevelNumber.Value);
        }
    }
}