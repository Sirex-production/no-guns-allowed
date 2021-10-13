using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, Projectile
    {
        [SerializeField] [Min(0)] private int maxNumberOfBounces = 0;
        [SerializeField] [Min(0)] private float speed = 1;

        private Vector3 _flyingDirection = Vector3.zero;
        private int _bounceCount = 0;

        private void FixedUpdate()
        {
            transform.position += _flyingDirection * speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_bounceCount >= maxNumberOfBounces)
            {
                Destroy(gameObject); //todo play destruction VFX
                return;
            }

            var contactPoint = other.GetContact(0);
            var bulletDirectionRelativeToTheSurface = Vector3.Normalize(contactPoint.point - transform.position);
            
            _flyingDirection = Vector3.Reflect(bulletDirectionRelativeToTheSurface, contactPoint.normal);
            _bounceCount++;
   
            //todo apply damage to others
        }
        
        //todo ignore actors from the input array
        public void Launch(Transform destination, params ActorStats[] ignoreHitActors)
        {
            transform.LookAt(destination);
            _flyingDirection = Vector3.Normalize(destination.position - transform.position);
        }

        //todo ignore actors from the input array
        public void Launch(Vector3 direction, params ActorStats[] ignoreHitActors)
        {
            _flyingDirection = Vector3.Normalize(direction);
        }
    }
}