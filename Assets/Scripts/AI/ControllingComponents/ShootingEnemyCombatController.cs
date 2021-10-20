using System;
using System.Collections;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class ShootingEnemyCombatController : MonoBehaviour, ICombatable
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] [Min(0)] private float meleeDamage = 5f;
        [Space] 
        [Tooltip("Controls whether AI will shoot if the opponent cannot be seen directly or not")]
        [SerializeField] private bool ignoreBarriers = true;
        [SerializeField] [Min(0)] private float pauseBetweenShots = 1;

        private AiBehaviourController _aiBehaviourController;
        private bool _isInCombat = false;
        
        private WaitForSeconds _pause;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            _pause = new WaitForSeconds(pauseBetweenShots);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isInCombat)
                return;

            //todo replace with enemy list
            if (other.transform.TryGetComponent(out PlayerStatsController player)) 
                player.TakeDamage(meleeDamage);
        }

        private IEnumerator ShootRoutine(ActorStats actorStats)
        {
            yield return _pause;
            
            while (_isInCombat)
            {
                if (actorStats == null)
                {
                    yield return null;
                    continue;
                }

                Bullet bullet;
                
                if (!ignoreBarriers)
                {
                    var direction = Vector3.Normalize(actorStats.transform.position - transform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(transform.position, direction, out hit))
                    {
                        if (hit.collider.transform == actorStats.transform)
                        {
                            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                            bullet.Launch(actorStats.transform, _aiBehaviourController.AiActorStats);

                            yield return _pause;
                            continue;
                        }
                    }

                    yield return null;
                    continue;
                }
                
                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.Launch(actorStats.transform, _aiBehaviourController.AiActorStats);
                
                yield return _pause;
            }
        }

        public void Attack(ActorStats actorStats)
        {
            _isInCombat = true;

            StartCoroutine(ShootRoutine(actorStats));
        }

        public void StopCombat()
        {
            _isInCombat = false;
        }
    }
}