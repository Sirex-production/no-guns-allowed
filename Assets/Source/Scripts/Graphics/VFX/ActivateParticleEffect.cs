using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Graphics.VFX
{
    public class ActivateParticleEffect : Effect
    {
        [Required] 
        [SerializeField] private ParticleSystem particleToActivate;
        
        public override void PlayEffect(Transform instanceTargetTransform)
        {
            particleToActivate.transform.position = instanceTargetTransform.position;
            particleToActivate.Play();
        }
    }
}