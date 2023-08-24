using Support.SLS;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Support
{
    public class TemplateManager : MonoBehaviour
    {
        [SerializeField] private int targetFpsOnCurrentScene = 60;
        [SerializeField] private VibrationMode vibrationMode = VibrationMode.Universal;

        [Inject] private SaveLoadSystem _saveLoadSystem;
        
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
            _frameCountStart = Time.frameCount;

            this.DoAfterNextFrameCoroutine(InitializeSystems);
        }

        private void Update()
        {
            _framesPerSecondSum += 1 / Time.deltaTime * Time.timeScale;

            if(Input.GetKeyDown(KeyCode.Space))
                print(AverageFPS);
        }

        private void InitializeSystems()
        {
            var saveData = _saveLoadSystem.SaveData;
            
            if(saveData.IsVibrationEnabled.Value)
                VibrationController.SetMode(vibrationMode);
            else
                VibrationController.SetMode(VibrationMode.Disabled);
        }
    }
}