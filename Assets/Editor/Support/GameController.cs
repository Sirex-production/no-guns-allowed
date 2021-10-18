using System;

namespace Support
{
    public class GameController : MonoSingleton<GameController>
    {
        public event Action<bool> OnLevelEnded;
        public event Action OnLevelRestart;

        public void RestartLevel()
        {
            OnLevelRestart?.Invoke();
        }

        public void EndLevel(bool isVictory)
        {
            OnLevelEnded?.Invoke(isVictory);
        }
    }
}