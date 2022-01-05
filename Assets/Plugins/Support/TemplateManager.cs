using UnityEngine;

namespace Support
{
    public class TemplateManager : MonoSingleton<TemplateManager>
    {
        [SerializeField] private int targetFpsOnCurrentScene = 60;
        [SerializeField] private VibrationMode vibrationMode = VibrationMode.Universal;

        private double _framesPerSecondSum = 0;
        private float _frameCountStart;
        
        public float AverageFPS
        {
            get
            {
                var secondsPassedFromStart =  Time.frameCount - _frameCountStart;
                return (float)_framesPerSecondSum / secondsPassedFromStart;
            }
        }

        private void Start()
        {
            Application.targetFrameRate = targetFpsOnCurrentScene;
            VibrationController.SetMode(vibrationMode);

            _frameCountStart = Time.frameCount;
        }

        private void Update()
        {
            _framesPerSecondSum += 1 / Time.deltaTime * Time.timeScale;

            if(Input.GetKeyDown(KeyCode.Space))
                print(AverageFPS);
        }
    }
}