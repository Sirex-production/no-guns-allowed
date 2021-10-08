using System.Collections;
using Support;
using UnityEngine;

public class PlayerGhost : PoolObject
{
    [SerializeField] private float timeToLive;

    private IEnumerator DeactivationDelayRoutine(float ttl)
    {
        yield return new WaitForSeconds(ttl);

        this.transform.gameObject.SetActive(false);
    }

    public void Destroy(float ttl)
    {
        StartCoroutine(DeactivationDelayRoutine(ttl));
    }

    public override void OnObjectReuse()
    {
        Destroy(timeToLive);
    }
}
