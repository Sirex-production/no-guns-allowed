using System;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class SectionTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private int boundedSection = -1;
        
        [Inject]
        private SectionsManager _sectionsManager;

        private void OnTriggerExit(Collider other)
        {
            if(_sectionsManager.CurrentSection == boundedSection)
                return;
            
            if (other.TryGetComponent(out PlayerEventController player))
                _sectionsManager.EnterSection(boundedSection);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var style = GUI.skin.box;
            Handles.Label(transform.position, $"Transition to section #{boundedSection}", style);
            
            Gizmos.color = new Color(0.16f, 1f, 0.04f);
            var attachedCollider = GetComponent<Collider>();

            if (attachedCollider is SphereCollider sphereCollider)
            {
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
                return;
            }

            if (attachedCollider is BoxCollider boxCollider)
            {
                Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
                return;
            }

            if (attachedCollider is MeshCollider meshCollider)
            {
                Gizmos.DrawWireMesh(meshCollider.sharedMesh);
            }
        }
#endif
    }
}