using UnityEngine;

namespace Ingame.Graphics
{
    public class BloodSprayEffect : Effect
    {
        [SerializeField] private ParticleSystem[] bloodParticles;

        private void Awake()
        {
            if(bloodParticles == null || bloodParticles.Length < 1)
                return;

            foreach (var bloodParticle in bloodParticles)
            {
                if(bloodParticle == null)
                    continue;
                
                bloodParticle.Stop();
                bloodParticle.Clear();
            }
        }

        public override void PlayEffect()
        {
            if(bloodParticles == null || bloodParticles.Length < 1)
                return;

            foreach (var bloodParticle in bloodParticles)
            {
                if(bloodParticle == null)
                    continue;
                
                bloodParticle.Play();
            }
        }
    }
}