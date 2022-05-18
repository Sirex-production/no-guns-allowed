using NaughtyAttributes;
using Support.Sound;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class AudioInstaller : MonoInstaller
    {
        [Required]
        [SerializeField] private AudioManager audioManager;
        
        public override void InstallBindings()
        {
            Container.Bind<AudioManager>()
                .FromInstance(audioManager)
                .AsSingle();
        }
    }
}