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
            this.SafeDebug("Shoot");

            while (_isInCombat)
            {
                var bullet = Instantiate(bulletPrefab, transform.position,
                    Quaternion.LookRotation(actorStats.transform.position - transform.position));

                bullet.Launch(actorStats.transform);


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