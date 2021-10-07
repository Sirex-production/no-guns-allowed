using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class TrailController : MonoBehaviour
    {
        [SerializeField] private float spawnPeriod;
        [SerializeField] private float timeToLive;
        [SerializeField] private GameObject echoPrefab;
        [SerializeField] private TrailRenderer primaryTrail;
        [SerializeField] private List<TrailRenderer> secondaryTrails;

        // Start is called before the first frame update
        private void Start()
        {
            primaryTrail.emitting = false;
            foreach (var trail in secondaryTrails)
                trail.emitting = true;
            
            PlayerEventController.Instance.OnDashPerformed += StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop += StopSpawningGhosts;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop -= StopSpawningGhosts;
        }

        private void SwitchActiveTrails()
        {
            primaryTrail.emitting = !primaryTrail.emitting;
            foreach (var trail in secondaryTrails)
                trail.emitting = !trail.emitting;
        }

        private void StartSpawningGhosts(Vector3 _)
        {
            StartCoroutine(SpawnGhostsRoutine());
        }

        private void StopSpawningGhosts()
        {
            SwitchActiveTrails();
            StopAllCoroutines();
        }


        private IEnumerator SpawnGhostsRoutine()
        {
            SwitchActiveTrails();
            while (true)
            {
                var go = Instantiate(echoPrefab);
                go.transform.position = this.transform.position;
                Destroy(go, timeToLive);

                yield return new WaitForSeconds(spawnPeriod);
            }
        }
    }
}
