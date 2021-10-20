using System;

namespace Support
{
    public class GameController : MonoSingleton<GameController>
    {
        public event Action<bool> OnLevelEnded;
        public event Action OnLevelRestart;

        private bool _isLevelEnded = false;
        
        public void RestartLevel()
        {
            OnLevelRestart?.Invoke();
            LevelManager.Instance.RestartLevel();
        }

        public void EndLevel(bool isVictory)
        {
            if(_isLevelEnded)
                return;

            _isLevelEnded = true;
            
            OnLevelEnded?.Invoke(isVictory);
        }
    }
}