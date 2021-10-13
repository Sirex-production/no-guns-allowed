using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Bullet : MonoBehaviour, Projectile
    {
        [SerializeField] [Min(0)] private int maxNumberOfBounces = 0;
        [SerializeField] [Min(0)] private float speed = 1;

        private Vector3 _flyingDirection = Vector3.zero;
        private int _bounceCount = 0;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _rigidbody.useGravity = false;

            Launch(-transform.right);
        }

        private void FixedUpdate()
        {
            _rigidbody.position += _flyingDirection * speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_bounceCount >= maxNumberOfBounces)
            {
                Destroy(gameObject); //todo play destruction VFX
                return;
            }

            var contactPoint = other.GetContact(0);
            var bulletDirectionRelativeToTheSurface = Vector3.Normalize(contactPoint.point - _rigidbody.position);
            
            _flyingDirection = Vector3.Reflect(bulletDirectionRelativeToTheSurface, contactPoint.normal);
            _bounceCount++;
   
            //todo apply damage to others
        }

        public void Launch(Transform destination)
        {
            _flyingDirection = Vector3.Normalize(destination.position - _rigidbody.position);
        }

        public void Launch(Vector3 direction)
        {
            _flyingDirection = Vector3.Normalize(direction);
        }
    }
}