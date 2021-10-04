using System;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class EchoEffectTime : MonoBehaviour
    {
        [SerializeField] private float spawnPeriod;
        [SerializeField] private float timeToLive;
        [SerializeField] private GameObject echoPrefab;

        private PlayerMovementController _pmcComponent;
        private float _timeElapsed;

        // Start is called before the first frame update
        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += ResetTime;

            _pmcComponent = GetComponent<PlayerMovementController>();
            _timeElapsed = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_pmcComponent.IsDashing) return;
            _timeElapsed += Time.deltaTime;

            if (!(_timeElapsed >= spawnPeriod)) return;

            var go = Instantiate(echoPrefab);
            go.transform.position = this.transform.position;
            Destroy(go, timeToLive);
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= ResetTime;
        }

        private void ResetTime(Vector3 _)
        {
            _timeElapsed = 0f;
        }
    }
}
