using NaughtyAttributes;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public class UiPauseMenu : MonoBehaviour
    {
        [Required]
        [SerializeField] private Slider sensitivitySlider;

        private void Start()
        {
            sensitivitySlider.value = SaveLoadSystem.Instance.SaveData.AimSensitivity.Value;
        }
    }
}