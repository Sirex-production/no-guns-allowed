using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class OpenCloseGatesAnimationFunctions : MonoBehaviour
    {
        private void LoadNextLevel()
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}