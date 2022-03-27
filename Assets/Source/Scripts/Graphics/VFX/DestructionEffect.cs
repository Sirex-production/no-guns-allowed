using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Ingame.Graphics
{
    public class DestructionEffect : Effect
    {
        [SerializeField] [Min(0)] private float destructionForce = 1;
        [SerializeField] [Min(0)] private float timeToSwitchLayer;
        [SerializeField] private List<Rigidbody> destructionParts;

        private const float PAUSE_BETWEEN_REMOVING_PARTS = .3f; 
        
        protected override void Start()
        {
            this.WaitAndDoCoroutine(timeAfterEffectWillBeDestroyed, RemoveComponents);
        }

        public override void PlayEffect(Transform instanceTargetTransform)
        {
            transform.localScale = instanceTargetTransform.transform.lossyScale / 1.2f;

            foreach (var destructionPart in destructionParts)
            {
                if(destructionPart == null)
                    continue;

                var forceDirection = Vector3.Normalize(transform.position - PlayerEventController.Instance.transform.position);
                forceDirection.z = 0;

                destructionPart.transform.parent = null;
                destructionPart.AddForce(forceDirection * destructionForce, ForceMode.Impulse);
            }
            
            this.WaitAndDoCoroutine(timeToSwitchLayer, SwitchLayer);
        }

        private void SwitchLayer()
        {
            var newLayer = LayerMask.NameToLayer("Ignore Collision With Dashing Player");
            foreach (var destructionPart in destructionParts)
                destructionPart.gameObject.layer = newLayer;
        }

        private IEnumerator RemoveComponentsRoutine()
        {
            foreach (var destructionPart in destructionParts)
            {
                if(destructionPart == null)
                    continue;
                
                Destroy(destructionPart.gameObject);

                yield return new WaitForSeconds(PAUSE_BETWEEN_REMOVING_PARTS);
            }
            
            Destroy(gameObject);
        }

        private void RemoveComponents()
        {
            StartCoroutine(RemoveComponentsRoutine());
        }
    }
}

