using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class UiPlayerDashesController : MonoBehaviour
    {
        [SerializeField] private UiCharge[] uiCharges;

        public void SetNumberOfActiveCharges(int numberOfActiveCharges)
        {
            for (var i = 0; i < uiCharges.Length; i++)
            {
                if (i < numberOfActiveCharges)
                    uiCharges[i].PlayRegenerateAnimation();
                else
                    uiCharges[i].PlayDisappearanceAnimation();
            }
        }

        public void HideCharges()
        {
            foreach (var uiCharge in uiCharges)
                uiCharge.GetComponent<Image>().enabled = false;
        }

        public void ShowCharges()
        {
            foreach (var uiCharge in uiCharges)
                uiCharge.GetComponent<Image>().enabled = true;
        }
    }
}