using System.Collections;
using Support;
using Zenject;
using UnityEngine;

namespace Ingame.Graphics
{
    public class AfterimageController : MonoBehaviour
    {
        [SerializeField] private float spawnPeriod;
        [SerializeField] private GameObject ghostPrefab;

        [Inject] private PoolManager _poolManager;

        private void Start()
        {
            _poolManager.CreatePool(ghostPrefab, 7);
            
            PlayerEventController.Instance.OnDashPerformed += StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop += StopSpawningGhosts;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= StartSpawningGhosts;
            PlayerEventController.Instance.OnDashStop -= StopSpawningGhosts;
        }

        private void StartSpawningGhosts(Vector3 _)
        {
            StartCoroutine(SpawnGhostsRoutine());
        }

        private void StopSpawningGhosts()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnGhostsRoutine()
        {
            while (true)
            {
                _poolManager.ReuseObject(ghostPrefab, this.transform.position);

                yield return new WaitForSeconds(spawnPeriod);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
