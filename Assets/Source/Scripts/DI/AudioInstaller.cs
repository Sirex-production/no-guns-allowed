using Ingame.Sound;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class AudioInstaller : MonoInstaller
    {
        [Required]
        [SerializeField] private LegacyAudioManager legacyAudioManager;
        
        public override void InstallBindings()
        {
            Container.Bind<LegacyAudioManager>()
                .FromInstance(legacyAudioManager)
                .AsSingle();
        }
    }
}