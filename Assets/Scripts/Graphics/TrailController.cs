using System.Collections;
using System.Collections.Generic;
using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class TrailController : MonoBehaviour
    {
        [SerializeField] private float spawnPeriod;
        [SerializeField] private GameObject ghostPrefab;
        [SerializeField] private TrailRenderer primaryTrail;
        [SerializeField] private List<TrailRenderer> secondaryTrails;

        // Start is called before the first frame update
        private void Start()
        {
            primaryTrail.emitting = false;
            foreach (var trail in secondaryTrails)
                trail.emitting = true;

            PoolManager.Instance.CreatePool(ghostPrefab, 7);
            
            PlayerEventController.Instance.OnDashPerformed += StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop += StopSpawningGhosts;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop -= StopSpawningGhosts;
        }

        private void SwitchActiveTrails(bool primaryTrailEmittingValue)
        {
            primaryTrail.emitting = primaryTrailEmittingValue;
            foreach (var trail in secondaryTrails)
                trail.emitting = !trail.emitting;
        }

        private void StartSpawningGhosts(Vector3 _)
        {
            StartCoroutine(SpawnGhostsRoutine());
        }

        private void StopSpawningGhosts()
        {
            SwitchActiveTrails(false);
            StopAllCoroutines();
        }


        private IEnumerator SpawnGhostsRoutine()
        {
            SwitchActiveTrails(true);
            while (true)
            {
                PoolManager.Instance.ReuseObject(ghostPrefab, this.transform.position);

                yield return new WaitForSeconds(spawnPeriod);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
