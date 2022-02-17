using Ingame.AI;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class LevelInstaller : MonoInstaller
    {
        [Required]
        [SerializeField] private ActorManager actorManager;
        [Required]
        [SerializeField] private SectionsManager sectionsManager;
        
        public override void InstallBindings()
        {
            Container.Bind<ActorManager>()
                .FromInstance(actorManager)
                .AsSingle();

            Container.Bind<SectionsManager>()
                .FromInstance(sectionsManager)
                .AsSingle();
        }
    }
}