using NaughtyAttributes;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class UiController : MonoSingleton<UiController>
    {
        [Required] [SerializeField] private UiPlayerDashesController uiDashesController;
        
        public UiPlayerDashesController UiDashesController => uiDashesController;
    }
}