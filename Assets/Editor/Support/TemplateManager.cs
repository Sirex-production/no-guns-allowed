using UnityEngine;

namespace Support
{
    public class TemplateManager : MonoSingleton<TemplateManager>
    {
        [SerializeField] private int targetFpsOnCurrentScene = 60;
        
        private void Start()
        {
            Application.targetFrameRate = targetFpsOnCurrentScene;
        }
    }
}