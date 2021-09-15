using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class UiPlayerDashesController : MonoBehaviour
    {
        [SerializeField] private Image dashesDisplayBar;

        public void SetNumberOfCharges(int currentNumberOfDashes, int maxNumberOfDashes)
        {
            var displayValue = Mathf.InverseLerp(0, maxNumberOfDashes, currentNumberOfDashes);
            dashesDisplayBar.fillAmount = displayValue;
        }
    }
}