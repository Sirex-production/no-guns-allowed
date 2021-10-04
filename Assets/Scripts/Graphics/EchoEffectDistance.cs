using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(Rigidbody))]
    public class EchoEffectDistance : MonoBehaviour
    {
        [SerializeField] private float spawnDistance;
        [SerializeField] private float timeToLive;
        [SerializeField] private GameObject echoPrefab;

        private PlayerMovementController _pmcComponent;
        private Rigidbody _rbComponent;
        private float _distanceTraveled;

        // Start is called before the first frame update
        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += ResetDistance;

            _pmcComponent = GetComponent<PlayerMovementController>();
            _rbComponent = GetComponent<Rigidbody>();
            _distanceTraveled = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_pmcComponent.IsDashing) return;
            _distanceTraveled += _rbComponent.velocity.magnitude * Time.deltaTime;

            if (!(_distanceTraveled >= spawnDistance)) return;

            var go = Instantiate(echoPrefab);
            go.transform.position = this.transform.position;
            Destroy(go, timeToLive);
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= ResetDistance;
        }

        private void ResetDistance(Vector3 _)
        {
            _distanceTraveled = 0f;
        }
    }
}
