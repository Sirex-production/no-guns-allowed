using System;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Support
{
    public class GameController : MonoBehaviour
    {
        public event Action<bool> OnLevelEnded;
        public event Action OnNextLevelLoaded;
        public event Action OnLevelRestart;

        private bool _isLevelEnded = false;

        public void EndLevel(bool isVictory)
        {
            if(_isLevelEnded)
                return;

            _isLevelEnded = true;
            
            VibrationController.Vibrate(!isVictory ? HapticTypes.Failure : HapticTypes.Success);

            OnLevelEnded?.Invoke(isVictory);
        }

        public void LoadNextLevel()
        {
            OnNextLevelLoaded?.Invoke();
        }

        public void RestartLevel()
        {
            OnLevelRestart?.Invoke();
        }
    }
}