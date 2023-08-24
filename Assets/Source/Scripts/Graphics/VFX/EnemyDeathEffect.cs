using Support.Extensions;
using UnityEngine;

namespace Ingame.Graphics
{
    public class EnemyDeathEffect : Effect
    {
        [Space]
        [SerializeField] [Min(0)] private float bodyPartsForce = 5f; 
        [Space]
        [SerializeField] private ParticleSystem[] particles;
        [SerializeField] private Rigidbody[] bodyParts;

        private void Awake()
        {
            if(particles == null || particles.Length < 1)
                return;

            foreach (var bloodParticle in particles)
            {
                if(bloodParticle == null)
                    continue;
                
                bloodParticle.Stop();
                bloodParticle.Clear();
            }
        }

        public override void PlayEffect(Transform instanceTargetTransform)
        {
            if (particles != null && particles.Length > 0)
                foreach (var bloodParticle in particles)
                {
                    if (bloodParticle == null)
                        continue;

                    bloodParticle.Play();
                }
            
            if(bodyParts != null && bodyParts.Length > 0)
                foreach (var body in bodyParts)
                {
                    if(body == null)
                        continue;

                    var directionModifier = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left; 
                    body.AddForce((VectorExtensions.RandomDirection() + directionModifier) * bodyPartsForce, ForceMode.Impulse);
                }
        }
    }
}