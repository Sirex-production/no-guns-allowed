using System.Collections;
using System.Collections.Generic;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class TrailController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer primaryTrail;
        [SerializeField] private List<TrailRenderer> secondaryTrails;

        private void Start()
        {
            SwitchActiveTrails(false);

            PlayerEventController.Instance.OnDashPerformed += StartPrimaryTrail;
            PlayerEventController.Instance.OnDashStop += StopPrimaryTrail;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= StartPrimaryTrail;
            PlayerEventController.Instance.OnDashStop -= StopPrimaryTrail;
        }

        private void SwitchActiveTrails(bool primaryTrailEmittingValue)
        {
            if(primaryTrail != null)
                primaryTrail.emitting = primaryTrailEmittingValue;
            foreach (var trail in secondaryTrails)
                trail.emitting = !primaryTrailEmittingValue;
        }

        private void StartPrimaryTrail(Vector3 _)
        {
            SwitchActiveTrails(true);

        }

        private void StopPrimaryTrail()
        {
            SwitchActiveTrails(false);
        }
    }
}
