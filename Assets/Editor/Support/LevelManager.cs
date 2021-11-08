using System;
using Support.SLS;
using UnityEngine.SceneManagement;

namespace Support
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public void ManageLevelDependingOnWinningCondition(bool isVictory)
        {
            if (isVictory)
                LoadNextLevel();
            else
                RestartLevel();
        }

        public void LoadLevel(int levelNumber)
        {
            if (levelNumber < 0)
                throw new ArgumentException($"There is no level with such index \"{levelNumber}\"");

            var sceneIndex = levelNumber < SceneManager.sceneCountInBuildSettings - 1
                ? levelNumber
                : levelNumber % SceneManager.sceneCountInBuildSettings;
            
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>Restarts last level that was saved in progress(SaveLoadSystem)</summary>
        public void RestartLevel()
        {
            LoadLevel(SaveLoadSystem.Instance.SaveData.CurrentLevelNumber.Data);
        }
        
        /// <summary>Loads next level and modifies progress in SaveLoadSystem</summary>
        public void LoadNextLevel()
        {
            SaveLoadSystem.Instance.SaveData.CurrentLevelNumber.Data++;
            SaveLoadSystem.Instance.PerformSave();
            
            LoadLevel(SaveLoadSystem.Instance.SaveData.CurrentLevelNumber.Data);
        }
    }
}