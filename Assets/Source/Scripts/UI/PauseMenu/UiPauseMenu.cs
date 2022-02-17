using NaughtyAttributes;
using Support.SLS;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI
{
    public class UiPauseMenu : MonoBehaviour
    {
        [Required]
        [SerializeField] private Slider sensitivitySlider;

        [Inject] private SaveLoadSystem _saveLoadSystem;
        
        private void Start()
        {
            sensitivitySlider.value = _saveLoadSystem.SaveData.AimSensitivity.Value;
        }
    }
}