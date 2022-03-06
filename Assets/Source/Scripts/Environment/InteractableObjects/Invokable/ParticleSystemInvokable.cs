using UnityEngine;

namespace Ingame
{
    public class ParticleSystemInvokable : MonoInvokable
    {
        [SerializeField] private bool isActivatedOnInvoke = true;
        [SerializeField] private ParticleSystem[] particleSystems;

        public override void Invoke()
        {
            foreach (var particleSystem in particleSystems)
            {
                if(particleSystem == null)
                    continue;
                
                particleSystem.Play();
            }
            
            base.Invoke();
        }
    }
}