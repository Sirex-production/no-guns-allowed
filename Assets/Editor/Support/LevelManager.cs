using System;
using UnityEngine.SceneManagement;

namespace Support
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        private void Start()
        {
            GameController.Instance.OnLevelEnded += ManageLevelWhenLevelEnds;
            GameController.Instance.OnLevelRestart += RestartLevel;
        }

        private void OnDestroy()
        {
            GameController.Instance.OnLevelEnded -= ManageLevelWhenLevelEnds;
            GameController.Instance.OnLevelRestart -= RestartLevel;
        }

        private void ManageLevelWhenLevelEnds(bool isWin)
        {
            if (isWin)
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

        private void RestartLevel()
        {
            LoadLevel(SaveLoadSystem.Instance.SaveData.currentLevel);
        }
        
        public void LoadNextLevel()
        {
            SaveLoadSystem.Instance.SaveData.currentLevel++;
            LoadLevel(SaveLoadSystem.Instance.SaveData.currentLevel);
        }
    }
}