using UnityEngine;

namespace Ingame.UI
{
    public class UiPlayerDashesController : MonoBehaviour
    {
        [SerializeField] private UiCharge[] _uiCharges;

        public void SetNumberOfActiveCharges(int numberOfActiveCharges)
        {
            for (var i = 0; i < _uiCharges.Length; i++)
            {
                if (i < numberOfActiveCharges)
                    _uiCharges[i].PlayRegenerateAnimation();
                else
                    _uiCharges[i].PlayDisappearanceAnimation();
            }
        }
    }
}