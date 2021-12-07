using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Graphics
{
    public class DestructionEffect : Effect
    {
        [SerializeField] [Min(0)] private float destructionForce = 1;
        [SerializeField] [Min(0)] private float timeToSwitchLayer;
        [SerializeField] private List<Rigidbody> destructionParts;

        public override void PlayEffect(Transform instanceTargetTransform)
        {
            transform.localScale = instanceTargetTransform.transform.lossyScale;

            foreach (var destructionPart in destructionParts)
            {
                if(destructionPart == null)
                    continue;

                var forceDirection = Vector3.Normalize(transform.position - PlayerEventController.Instance.transform.position);
                forceDirection.z = 0;

                destructionPart.transform.parent = null;
                destructionPart.AddForce(forceDirection * destructionForce, ForceMode.Impulse);
            }

            Invoke(nameof(SwitchLayer), timeToSwitchLayer);
        }

        private void SwitchLayer()
        {
            foreach (var destructionPart in destructionParts)
                destructionPart.gameObject.layer = 6;
        }
    }
}

