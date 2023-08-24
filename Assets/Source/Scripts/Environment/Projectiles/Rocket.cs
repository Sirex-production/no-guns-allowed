using System;
using Support.Extensions;
using Ingame.AI;
using Ingame.Graphics;
using UnityEngine;

namespace Ingame
{
    public class Rocket : Projectile
    {
        [SerializeField] [Min(0)] private float damage = 10f;
        [SerializeField] [Min(0)] private float damageRadius = 1f;
        [SerializeField] [Min(0)] private float delayBeforeRocketCanDealDamage = .5f;
        [Space]
        [SerializeField] [Min(0)] private float movementSpeed = 8f;
        [SerializeField] [Min(0)] private float rotationSpeed = 2f;

        private const int MAXIMUM_NUMBER_OF_COLLIDERS_DETECTED = 10;

        private EffectsFactory _effectsFactory;
        private Rigidbody _rigidbody;
        private bool _canDealDamage = false;
        private Transform _target;

        private void Awake()
        {
            _effectsFactory = GetComponent<EffectsFactory>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                var lookDirection = _target.transform.position - _rigidbody.transform.position;
                var lookRotation = Quaternion.LookRotation(lookDirection);

                _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                var rotationCopy = _rigidbody.rotation.eulerAngles;
                // rotationCopy.x = 0;
                // rotationCopy.y = 0;
                // rotationCopy.z = 0;
                
                _rigidbody.rotation = Quaternion.Euler(rotationCopy);
            }

            _rigidbody.position += transform.forward * movementSpeed * Time.fixedDeltaTime;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!_canDealDamage)
                return;
            
            if (!other.isTrigger)
            {
                BlowUp();
                return;
            }

            if (other.TryGetComponent(out HitBox _)) 
                BlowUp();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }

        private void BlowUp()
        {
            Collider[] hitColliders = new Collider[MAXIMUM_NUMBER_OF_COLLIDERS_DETECTED];
            if (Physics.OverlapSphereNonAlloc(transform.position, damageRadius, hitColliders) > 0)
                foreach (var collider in hitColliders)
                {
                    if(collider == null)
                        continue;
                    
                    if (collider.TryGetComponent(out HitBox hitBox))
                        hitBox.TakeDamage(damage, DamageType.Explosion);
                }
            
            _effectsFactory.PlayAllEffects(EffectType.Destruction);

            Destroy(gameObject);
        }

        public override void Launch(Transform destination, params ActorStats[] ignoreHitActors)
        {
            _target = destination;
            
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position);

            this.WaitAndDoCoroutine(delayBeforeRocketCanDealDamage, () => _canDealDamage = true);
        }

        public override void Launch(Vector3 direction, params ActorStats[] ignoreHitActors)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            
            this.WaitAndDoCoroutine(delayBeforeRocketCanDealDamage, () => _canDealDamage = true);
        }
    }
}