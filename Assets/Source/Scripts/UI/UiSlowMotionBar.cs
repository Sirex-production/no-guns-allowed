using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    [RequireComponent(typeof(Slider))]
    public class UiSlowMotionBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image hourGlassIcon;
        [SerializeField] private Color minValueColor;
        [SerializeField] private Color maxValueColor;

        private Color _hourGlassInactiveColor;
        private Color _hourGlassActiveColor;
        private Slider _slider;

        private void Awake()
        {
            _hourGlassActiveColor = new Color(0.68f, 0.68f, 0.68f);
            _hourGlassInactiveColor = new Color(0.67f, 0.31f, 0.31f);
        }

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0.0f;
            _slider.maxValue = SlowMotionController.Instance.SlowMotionPool;
        }

        //TODO: find a way to optimize the update rate of the bar. Of course coupling it to SMController doesn't count
        private void Update()
        {
            _slider.value = SlowMotionController.Instance.TimeRemaining;
            fillImage.color = Color.Lerp(minValueColor, maxValueColor, _slider.value / _slider.maxValue);
            hourGlassIcon.color = SlowMotionController.Instance.OutOfTime
                ? _hourGlassInactiveColor
                : _hourGlassActiveColor;
        }
    }
}
