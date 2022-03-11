using System.Collections;
using Extensions;
using Ingame.AI;
using Ingame.Graphics;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class LethalObject : MonoBehaviour
    {
        [SerializeField] [Layer] private int staticSurfacesLayer = 0;
        [SerializeField] private ParticleSystem[] particles;

        [Inject] private EffectsManager _effectsManager;

        private const float SHAKE_DURATION = 0.1f; 
        private const float DAMAGE = 1000f;
        private const float COMPONENT_REMOVAL_POLL_FREQUENCY = 1f;

        private bool _isInvoked;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox) ||
                hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController) 
                return;

            if (hitbox.AttachedActorStats.IsInvincible) 
                return;

            hitbox.TakeDamage(DAMAGE, DamageType.Environment);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != staticSurfacesLayer || _isInvoked)
                return;

            _isInvoked = true;
            foreach (var particle in particles)
            {
                if (particle != null)
                    particle.Play();
            }
            _effectsManager.ShakeEnvironment(SHAKE_DURATION);
            StartCoroutine(ComponentRemovalCoroutine());
        }

        private IEnumerator ComponentRemovalCoroutine()
        {
            var rigidBodyComponent = GetComponent<Rigidbody>();
            while (true)
            {
                if (rigidBodyComponent.velocity.sqrMagnitude > 0.01f)
                {
                    yield return new WaitForSeconds(COMPONENT_REMOVAL_POLL_FREQUENCY);
                    continue;
                }

                RemoveComponents();
                break;
            }
        }

        private void RemoveComponents()
        {
            foreach (var particle in particles)
            {
                if (particle != null)
                    Destroy(particle.gameObject);
            }
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(this);
        }
    }
}
