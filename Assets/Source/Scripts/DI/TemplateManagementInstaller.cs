using Ingame.UI;
using NaughtyAttributes;
using Support;
using Support.SLS;
using Support.Sound;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class TemplateManagementInstaller : MonoInstaller
    {
        [Required] 
        [SerializeField] private GameController gameController;
        [Required] 
        [SerializeField] private LevelManager levelManager;
        [Required] 
        [SerializeField] private SaveLoadSystem saveLoadSystem;
        [Required] 
        [SerializeField] private TemplateManager templateManager;
        [Required] 
        [SerializeField] private UiController uiController;
        [Required]
        [SerializeField] private AnalyticsWrapper analyticsWrapper;
        [Required]
        [SerializeField] private TutorialsManager tutorialsManager;
        [Required]
        [SerializeField] private InputSystem inputSystem;

        public override void InstallBindings()
        {
            Container.Bind<GameController>()
                .FromInstance(gameController)
                .AsSingle();
            
            Container.Bind<LevelManager>()
                .FromInstance(levelManager)
                .AsSingle();
            
            Container.Bind<SaveLoadSystem>()
                .FromInstance(saveLoadSystem)
                .AsSingle();
            
            Container.Bind<TemplateManager>()
                .FromInstance(templateManager)
                .AsSingle();
            
            Container.Bind<UiController>()
                .FromInstance(uiController)
                .AsSingle();
            
            Container.Bind<AnalyticsWrapper>()
                .FromInstance(analyticsWrapper)
                .AsSingle();

            Container.Bind<TutorialsManager>()
                .FromInstance(tutorialsManager)
                .AsSingle();

            Container.Bind<InputSystem>()
                .FromInstance(inputSystem)
                .AsSingle();
        }
    }
}