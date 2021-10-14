using System.Collections;
using Extensions;
using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class ShootingEnemyCombatController : MonoBehaviour, ICombatable
    {
        [SerializeField] private Bullet bulletPrefab;
        [Space] 
        [SerializeField] [Min(0)] private float pauseBetweenShots = 1;
        
        private AiBehaviourController _aiBehaviourController;
        
        private bool _isInCombat = false;

        private IEnumerator ShootRoutine(ActorStats actorStats)
        {
            while (_isInCombat)
            {
                //todo reduce hardcode (setup prober place for spawning bullet)
                 var bullet = Instantiate(bulletPrefab, transform.position + Vector3.down * 2.5f,
                    Quaternion.identity);

                bullet.Launch(actorStats.transform); //todo add stats to ignore
                
                yield return new WaitForSeconds(pauseBetweenShots);
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