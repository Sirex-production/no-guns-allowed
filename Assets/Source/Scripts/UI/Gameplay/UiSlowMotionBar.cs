using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    [RequireComponent(typeof(Slider))]
    public class UiSlowMotionBar : MonoBehaviour
    {
        [BoxGroup("References")] [SerializeField] private Image fillImage;
        [BoxGroup("References")] [SerializeField] private Image hourGlassIcon;
        [BoxGroup("References")] [SerializeField] private Slider slider;
        
        [BoxGroup("Colors")] [SerializeField] private Color minValueColor;
        [BoxGroup("Colors")] [SerializeField] private Color maxValueColor;
        [BoxGroup("Colors")] [SerializeField] private Color hourGlassInactiveColor;
        [BoxGroup("Colors")] [SerializeField] private Color hourGlassActiveColor;

        private void Start()
        {
            slider.minValue = 0.0f;
            slider.maxValue = SlowMotionController.Instance.SlowMotionPool;
        }

        //TODO: find a way to optimize the update rate of the bar. Of course coupling it to SMController doesn't count
        private void Update()
        {
            slider.value = SlowMotionController.Instance.TimeRemaining;
            fillImage.color = Color.Lerp(minValueColor, maxValueColor, slider.value / slider.maxValue);
            hourGlassIcon.color = SlowMotionController.Instance.OutOfTime ? hourGlassInactiveColor : hourGlassActiveColor;
        }
    }
}
