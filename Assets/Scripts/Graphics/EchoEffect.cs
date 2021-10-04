using Ingame;
using UnityEngine;


[RequireComponent(typeof(PlayerMovementController))]
public class EchoEffect : MonoBehaviour
{
    [SerializeField] private float spawnPeriod;
    [SerializeField] private float timeToLive;
    [SerializeField] private GameObject echoPrefab;

    private PlayerMovementController _pmcComponent;
    private float _timeElapsed;

    // Start is called before the first frame update
    private void Start()
    {
        _pmcComponent = GetComponent<PlayerMovementController>();
        _timeElapsed = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_pmcComponent.IsDashing) return;
        _timeElapsed += Time.deltaTime;

        if (!(_timeElapsed >= spawnPeriod)) return;
        _timeElapsed = 0f;
                
        var go = Instantiate(echoPrefab);
        go.transform.position = this.transform.position;
        Destroy(go, timeToLive);
    }
}
