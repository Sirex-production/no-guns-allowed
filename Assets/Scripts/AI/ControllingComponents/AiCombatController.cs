using System.Collections;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    [DisallowMultipleComponent]
    public class AiCombatController : MonoBehaviour, ICombatable
    {
        private AiBehaviourController _aiBehaviourController;
        private bool _isInCombat = false;
        
        private WaitForSeconds _pause;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            _pause = new WaitForSeconds(_aiBehaviourController.AiData.PauseBetweenShots);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isInCombat)
                return;

            //todo replace with enemy list
            if (other.transform.TryGetComponent(out PlayerStatsController player)) 
                player.TakeDamage(_aiBehaviourController.AiData.MeleeDamage);
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
                
                if (!_aiBehaviourController.AiData.IgnoreBarriers)
                {
                    var direction = Vector3.Normalize(actorStats.transform.position - transform.position);

                    if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
                    {
                        if (hit.collider.transform == actorStats.transform)
                        {
                            bullet = Instantiate(_aiBehaviourController.AiData.BulletPrefab, transform.position, Quaternion.identity);
                            bullet.Launch(actorStats.transform, _aiBehaviourController.AiActorStats);

                            yield return _pause;
                            continue;
                        }
                    }

                    yield return null;
                    continue;
                }
                
                bullet = Instantiate(_aiBehaviourController.AiData.BulletPrefab, transform.position, Quaternion.identity);
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