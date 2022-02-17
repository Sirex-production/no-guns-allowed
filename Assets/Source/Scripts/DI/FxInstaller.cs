using Ingame.Graphics;
using Ingame.Graphics.VFX;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.DI
{
    public class FxInstaller : MonoInstaller
    {
        [Required] 
        [SerializeField] private SlowMotionEffect slowMotionEffect;
        [Required] 
        [SerializeField] private PoolManager poolManager;
        [Required] 
        [SerializeField] private EffectsManager effectsManager;

        public override void InstallBindings()
        {
            Container.Bind<SlowMotionEffect>()
                .FromInstance(slowMotionEffect)
                .AsSingle();
            
            Container.Bind<PoolManager>()
                .FromInstance(poolManager)
                .AsSingle();
            
            Container.Bind<EffectsManager>()
                .FromInstance(effectsManager)
                .AsSingle();
        }
    }
}