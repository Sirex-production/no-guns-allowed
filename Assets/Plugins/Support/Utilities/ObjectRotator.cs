using UnityEngine;

namespace Support
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationDirectionalSpeed;



        private void FixedUpdate()
        {
            var localRotation = transform.localRotation;
            var targetRotation = Quaternion.Euler(rotationDirectionalSpeed) * localRotation;
               
            localRotation = Quaternion.Lerp(localRotation, targetRotation, Time.fixedDeltaTime);
            transform.localRotation = localRotation;
        }
    }
}