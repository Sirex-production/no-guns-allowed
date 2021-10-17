using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour, Projectile
    {
        [SerializeField] [Min(0)] private int maxNumberOfBounces = 0;
        [SerializeField] [Min(0)] private float speed = 1;
        [SerializeField] [Min(0)] private float damage = 1;

        private List<ActorStats> _ignoreHitActors;
        private Vector3 _flyingDirection = Vector3.zero;
        private int _bounceCount = 0;

        private void FixedUpdate()
        {
            transform.position += _flyingDirection * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            void ManageReflection()
            {
                if (other.isTrigger)
                    return;
                
                if (_bounceCount >= maxNumberOfBounces)
                {
                    Destroy(gameObject);
                    return;
                }
                
                var contactPoint = other.ClosestPoint(transform.position);
                var bulletDirectionRelativeToTheSurface = Vector3.Normalize(transform.position - contactPoint);

                Reflect(bulletDirectionRelativeToTheSurface);
                _bounceCount++;
            }

            if (other.TryGetComponent(out ActorStats actor))
            {
                if (_ignoreHitActors.Contains(actor))
                    return;
                
                if (!actor.IsInvincible)
                {
                    actor.TakeDamage(damage);
                    Destroy(gameObject);
                    return;
                }
                
                _ignoreHitActors.Clear();
                Reflect(-_flyingDirection);
                return;
            }

            ManageReflection();
        }

        public void Launch(Transform destination, params ActorStats[] ignoreHitActors)
        {
            _ignoreHitActors = new List<ActorStats>(ignoreHitActors);
            
            transform.LookAt(destination);
            _flyingDirection = Vector3.Normalize(destination.position - transform.position);
        }

        public void Launch(Vector3 direction, params ActorStats[] ignoreHitActors)
        {
            _ignoreHitActors = new List<ActorStats>(ignoreHitActors);
            
            transform.rotation = Quaternion.LookRotation(direction - transform.position);
            _flyingDirection = Vector3.Normalize(direction);
        }

        public void Reflect(Vector3 direction)
        {
            if (direction.magnitude == 0)
                direction = new Vector3(Random.Range(.1f, 1), Random.Range(.1f, 1), 0);
            
            direction = direction.normalized;

            _flyingDirection = direction;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}