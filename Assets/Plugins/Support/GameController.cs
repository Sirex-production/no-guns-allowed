using System;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Support
{
    public class GameController : MonoBehaviour
    {
        public event Action<bool> OnLevelEnded;
        public event Action<int> OnLevelLoaded;
        public event Action OnLevelRestart;
        public event Action OnLastPlayedLevelFromStaveLoaded;
        
        private bool _isLevelEnded = false;

        public void EndLevel(bool isVictory)
        {
            if(_isLevelEnded)
                return;

            _isLevelEnded = true;
            
            VibrationController.Vibrate(!isVictory ? HapticTypes.Failure : HapticTypes.Success);

            OnLevelEnded?.Invoke(isVictory);
        }

        public void RestartLevel()
        {
            OnLevelRestart?.Invoke();
        }

        public void LoadLevel(int levelNumber)
        {
            OnLevelLoaded?.Invoke(levelNumber);
        }

        public void LoadLastPlayedLevelFromSave()
        {
            OnLastPlayedLevelFromStaveLoaded?.Invoke();
        }
    }
}