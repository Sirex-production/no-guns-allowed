using System;
using System.Collections.Generic;
using Ingame.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class Bullet : Projectile
    {
        [SerializeField] [Min(0)] private int maxNumberOfBounces = 0;
        [SerializeField] [Min(0)] private float speed = 1;
        [SerializeField] [Min(0)] private float speedAfterPlayerReflection = 1;
        [SerializeField] [Min(0)] private float damage = 1;

        private List<ActorStats> _ignoreHitActors;
        private Vector3 _flyingDirection = Vector3.zero;
        private float _currentSpeed;
        private int _bounceCount = 0;

        public enum ReflectionType {Surface, Player}
        public event Action<ReflectionType> OnReflect;

        private void Awake()
        {
            _currentSpeed = speed;
        }

        private void FixedUpdate()
        {
            transform.position += _flyingDirection * _currentSpeed * Time.deltaTime;
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
                
                Reflect(bulletDirectionRelativeToTheSurface, ReflectionType.Surface);
                _bounceCount++;
            }

            if (other.TryGetComponent(out HitBox hitBox))
            {
                if (_ignoreHitActors.Contains(hitBox.AttachedActorStats))
                    return;

                if (!hitBox.AttachedActorStats.IsInvincible)
                {
                    hitBox.TakeDamage(damage, DamageType.Projectile);
                    Destroy(gameObject);
                    return;
                }

                _ignoreHitActors.Clear();
                _ignoreHitActors.Add(hitBox.AttachedActorStats);
                _bounceCount = maxNumberOfBounces;
                Reflect(-_flyingDirection, ReflectionType.Player);
                return;
            }
            
            if (other.TryGetComponent(out ActorStats actor) && _ignoreHitActors.Contains(actor))
                return;

            ManageReflection();
        }

        private void Reflect(Vector3 direction, ReflectionType reflectionType)
        {
            OnReflect?.Invoke(reflectionType);

            if (reflectionType == ReflectionType.Player) 
                _currentSpeed = speedAfterPlayerReflection;

            if (direction.magnitude == 0)
                direction = new Vector3(Random.Range(.1f, 1), Random.Range(.1f, 1), 0);
            
            direction = direction.normalized;

            _flyingDirection = direction;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        public override void Launch(Transform destination, params ActorStats[] ignoreHitActors)
        {
            _ignoreHitActors = new List<ActorStats>(ignoreHitActors);

            transform.LookAt(destination);
            _flyingDirection = Vector3.Normalize(destination.position - transform.position);
        }

        public override void Launch(Vector3 direction, params ActorStats[] ignoreHitActors)
        {
            _ignoreHitActors = new List<ActorStats>(ignoreHitActors);

            transform.rotation = Quaternion.LookRotation(direction - transform.position);
            _flyingDirection = Vector3.Normalize(direction);
        }
    }
}