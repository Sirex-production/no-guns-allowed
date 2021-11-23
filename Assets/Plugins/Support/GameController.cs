using System;
using MoreMountains.NiceVibrations;

namespace Support
{
    public class GameController : MonoSingleton<GameController>
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
            
            if(!isVictory)
                VibrationController.Vibrate(HapticTypes.Failure);
            
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