using Support;
using UnityEngine;

namespace Ingame
{
    public class UiController : MonoSingleton<UiController>
    {
        [SerializeField] private UiPlayerDashesController uiDashesController;

        public UiPlayerDashesController UiDashesController => uiDashesController;
    }
}