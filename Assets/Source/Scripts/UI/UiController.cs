using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class UiController : MonoSingleton<UiController>
    {
        [SerializeField] private UiPlayerDashesController uiDashesController;

        public UiPlayerDashesController UiDashesController => uiDashesController;
    }
}