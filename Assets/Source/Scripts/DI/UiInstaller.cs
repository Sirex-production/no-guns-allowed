using Ingame.UI;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class UiInstaller : MonoInstaller
    {
        [BoxGroup("References"), Required]
        [SerializeField] private UiNarrative uiNarrative;
        [SerializeField] private string version;
        [HorizontalLine(1f, EColor.Red)]
        [SerializeField] private string linkToFeedback;

        public override void InstallBindings()
        {
            Container.Bind<UiNarrative>()
                .FromInstance(uiNarrative)
                .AsSingle();
            
            Container.Bind<string>()
                .WithId("version")
                .FromInstance(version)
                .AsCached();
            
            Container.Bind<string>()
                .WithId("feedback-link")
                .FromInstance(linkToFeedback)
                .AsCached();
        }
    }
}