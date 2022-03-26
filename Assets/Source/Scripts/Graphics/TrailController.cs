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
        [SerializeField] private float spawnPeriod;
        [SerializeField] private GameObject ghostPrefab;
        [SerializeField] private TrailRenderer primaryTrail;
        [SerializeField] private List<TrailRenderer> secondaryTrails;

        [Inject] private PoolManager _poolManager;
        
        // Start is called before the first frame update
        private void Start()
        {
            if(primaryTrail != null)
                primaryTrail.emitting = false;
            foreach (var trail in secondaryTrails)
                trail.emitting = true;

            _poolManager.CreatePool(ghostPrefab, 7);
            
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
            if(primaryTrail != null)
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
                _poolManager.ReuseObject(ghostPrefab, this.transform.position);

                yield return new WaitForSeconds(spawnPeriod);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
