using UnityEngine;

namespace Support
{
    public class TemplateManager : MonoSingleton<TemplateManager>
    {
        [SerializeField] private int targetFpsOnCurrentScene = 60;
        [SerializeField] private VibrationMode vibrationMode = VibrationMode.Universal;
        
        private void Start()
        {
            Application.targetFrameRate = targetFpsOnCurrentScene;
            VibrationController.SetMode(vibrationMode);
        }
    }
}