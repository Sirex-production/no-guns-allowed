using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using Ingame.Graphics;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class SpaceCapsuleMover : MonoBehaviour
    {
        [Required] 
        [SerializeField] private Transform destinationTransform;
        [SerializeField] [Min(0)] private float animationDuration;
        [Space]
        [SerializeField] private List<MonoInvokable> invokeOnStartList;
        [SerializeField] private List<MonoInvokable> invokeAfterArrivalList;

        [Inject] private EffectsManager _effectsManager;

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
            ActivateInvokables(invokeOnStartList);
            this.DoAfterNextFrameCoroutine(PlayFlyAnimation);
        }

        private void PlayFlyAnimation()
        {
            _effectsManager.ShakeEnvironment(animationDuration * 1.3f);
            transform.DOMove(destinationTransform.position, animationDuration)
                .OnComplete(() => ActivateInvokables(invokeAfterArrivalList));
        }

        private void ActivateInvokables(IEnumerable<IInvokable> invokables)
        {
            foreach (var invokable in invokables)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }
    }
}