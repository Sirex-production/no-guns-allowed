using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class EchoEffectTime : MonoBehaviour
    {
        [SerializeField] private float spawnPeriod;
        [SerializeField] private float timeToLive;
        [SerializeField] private GameObject echoPrefab;

        // Start is called before the first frame update
        private void Start()
        {
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
                var go = Instantiate(echoPrefab);
                go.transform.position = this.transform.position;
                Destroy(go, timeToLive);

                yield return new WaitForSeconds(spawnPeriod);
            }
        }
    }
}
