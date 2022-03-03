using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class SpaceCapsuleMover : MonoBehaviour
    {
        [Required] 
        [SerializeField] private Transform destinationTransform;
        [SerializeField] [Min(0)] private float animationDuration;
        [Space]
        [SerializeField] private MonoInvokable[] invokeAfterArrivalList;

        private const float GIZMOS_SPHERE_RADIUS = .2f;
        
        private void OnDrawGizmos()
        {
            if(destinationTransform == null)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, destinationTransform.position);
            Gizmos.DrawSphere(destinationTransform.position, GIZMOS_SPHERE_RADIUS);
        }

        private void Start()
        {
            PlayFlyAnimation();
        }

        private void PlayFlyAnimation()
        {
            transform.DOMove(destinationTransform.position, animationDuration)
                .OnComplete(ActivateAfterArrival);
        }

        private void ActivateAfterArrival()
        {
            foreach (var invokable in invokeAfterArrivalList)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }
    }
}