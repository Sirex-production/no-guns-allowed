using System.Collections.Generic;
using Ingame.Graphics;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class ParticleActivator : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particlesToActivateDuringEnvironmentCheck;

        [Inject] private EffectsManager _effectsManager;

        private void Start()
        {
            _effectsManager.OnEnvironmentShake += OnEnvironmentShake;
        }

        private void OnDestroy()
        {
            _effectsManager.OnEnvironmentShake -= OnEnvironmentShake;
        }

        private void OnEnvironmentShake(float _)
        {
            ActivateParticles();
        }

        private void ActivateParticles()
        {
            foreach (var particleSystem in particlesToActivateDuringEnvironmentCheck)
            {
                if(particleSystem == null)
                    continue;
                
                particleSystem.Play();
            }
        }
    }
}