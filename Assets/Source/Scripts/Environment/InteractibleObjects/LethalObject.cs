using System;
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
        [SerializeField] private float timeToRemoveComponents = 5;

        private BoxCollider[] _children;

        private void Awake()
        {
            _children = transform.GetComponentsInChildren<BoxCollider>();
            
            this.WaitAndDoCoroutine(timeToRemoveComponents, RemoveComponents);
            this.WaitAndDoCoroutine(timeToSwitchLayer, SwitchLayer);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out HitBox hitbox) ||
                hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController) 
                return;

            if (!hitbox.AttachedActorStats.IsInvincible)
                hitbox.TakeDamage(damage);
        }

        private void SwitchLayer()
        {
            var newLayer = LayerMask.NameToLayer("Ignore Collision With Dashing Player");
            foreach (var child in _children)
                child.gameObject.layer = newLayer;
        }

        private void RemoveComponents()
        {
            //Destroy(gameObject.GetComponent<Rigidbody>());

            //foreach (var child in _children)
            //{
            //    child.TryGetComponent(out BoxCollider boxColliderComponent);
            //    Destroy(boxColliderComponent);
            //}
            
            Destroy(this);
        }
    }
}
