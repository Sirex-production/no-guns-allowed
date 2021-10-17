using System.Collections;
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

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
        }

        private IEnumerator ShootRoutine(ActorStats actorStats)
        {
            while (_isInCombat)
            {
                //todo add detection time before shot
                yield return new WaitForSeconds(pauseBetweenShots);
                
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                bullet.Launch(actorStats.transform, _aiBehaviourController.AiActorStats);
                
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