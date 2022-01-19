using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Ingame.AI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ingame
{
    public class LethalObject : MonoBehaviour
    {
        [SerializeField] private float damage = 1;
        [SerializeField] private float timeToSwitchLayer = 3;
        [SerializeField] private float componentRemovalPollFrequency = 1;

        private BoxCollider[] _children;

        private void Awake()
        {
            _children = transform.GetComponentsInChildren<BoxCollider>();
            
            this.WaitAndDoCoroutine(timeToSwitchLayer, SwitchLayer);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox) ||
                hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController) 
                return;

            if (hitbox.AttachedActorStats.IsInvincible) 
                return;

            StartCoroutine(ComponentRemovalCoroutine());
            hitbox.TakeDamage(damage);
        }

        private IEnumerator ComponentRemovalCoroutine()
        {
            var rigidBodyComponent = GetComponent<Rigidbody>();
            while (true)
            {
                if (!(rigidBodyComponent.velocity.sqrMagnitude < 0.01f))
                {
                    yield return new WaitForSeconds(componentRemovalPollFrequency);
                    continue;
                }

                RemoveComponents();
                break;
            }
        }

        private void SwitchLayer()
        {
            var newLayer = LayerMask.NameToLayer("Ignore Collision With Dashing Player");
            foreach (var child in _children)
                child.gameObject.layer = newLayer;
        }

        private void RemoveComponents()
        {
            Destroy(gameObject.GetComponent<Rigidbody>());

            foreach (var child in _children)
            {
                child.TryGetComponent(out BoxCollider boxColliderComponent);
                Destroy(boxColliderComponent);
            }

            Destroy(this);
        }
    }
}
