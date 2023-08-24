using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    [DisallowMultipleComponent]
    public class AiCombatController : MonoBehaviour, ICombatable
    {
        [Inject] private ActorManager _actorManager;

        private const float MAX_VISION_DISTANCE = 60f;
        
        private AiBehaviourController _aiBehaviourController;
        private bool _isInCombat = false;
        
        private WaitForSeconds _pause;
        private ActorStats[] _ignoreActorsForBullet;

        private float _timePassedFromEnemyLoss = 0f;
        
        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            _pause = new WaitForSeconds(_aiBehaviourController.AiData.PauseBetweenShots);
        }

        private void Start()
        {
            this.DoAfterNextFrameCoroutine(() => _ignoreActorsForBullet = GetAllFriendActors());
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_isInCombat)
                return;

            //todo replace with enemy list
            if (other.TryGetComponent(out HitBox enemy) && enemy.AttachedActorStats is PlayerStatsController) 
                enemy.TakeDamage(_aiBehaviourController.AiData.MeleeDamage, DamageType.Melee);
        }

        private IEnumerator ShootRoutine(ActorStats actorStats)
        {
            void RotateTowardsEnemy()
            {
                var targetRotation = Quaternion.LookRotation(actorStats.transform.position - transform.position);
                targetRotation.eulerAngles = Vector3.up * targetRotation.eulerAngles.y;
                _aiBehaviourController.AiMovementController.Rotate(targetRotation, _aiBehaviourController.AiData.RotationSpeed * 2);
            }

            RotateTowardsEnemy();

            yield return _pause;

            var raycastMask = ~LayerMask.GetMask("Ignore Raycast", "Projectile", "Breakable Object");

            while (_isInCombat)
            {
                if (actorStats == null)
                {
                    yield return null;
                    continue;
                }

                Projectile bullet;
                
                if (!_aiBehaviourController.AiData.IgnoreBarriers)
                {
                    var direction = Vector3.Normalize(actorStats.transform.position - transform.position);
                    
                    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, MAX_VISION_DISTANCE, raycastMask, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.collider.transform == actorStats.transform)
                        {
                            RotateTowardsEnemy();
                            bullet = Instantiate(_aiBehaviourController.AiData.ProjectilePrefab, transform.position, Quaternion.identity);
                            bullet.Launch(actorStats.transform, _ignoreActorsForBullet);
                            
                            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                                _aiBehaviourController.ShootingEnemyAnimator.Shoot();

                            _timePassedFromEnemyLoss = 0;
                            yield return _pause;
                            continue;
                        }
                    }

                    if (_timePassedFromEnemyLoss > _aiBehaviourController.AiData.EnterRestStateTime)
                    {
                        _aiBehaviourController.EnterRest();
                        _timePassedFromEnemyLoss = 0;
                        StopCombat();
                    }

                    _timePassedFromEnemyLoss += Time.deltaTime;
                    yield return null;
                    continue;
                }
                
                RotateTowardsEnemy();
                bullet = Instantiate(_aiBehaviourController.AiData.ProjectilePrefab, transform.position, Quaternion.identity);
                bullet.Launch(actorStats.transform, _ignoreActorsForBullet);
                
                if(_aiBehaviourController.ShootingEnemyAnimator != null)
                    _aiBehaviourController.ShootingEnemyAnimator.Shoot();
                
                yield return _pause;
            }
        }

        private ActorStats[] GetAllFriendActors()
        {
            var ignoreActors = new List<ActorStats>();
            var friends = _actorManager.GetOppositeActors(_aiBehaviourController.AiData.HostileSides.ToArray());
            
            ignoreActors.Add(_aiBehaviourController.AiActorStats);
            ignoreActors.AddRange(friends);

            return ignoreActors.ToArray();
        }

        public void Attack(ActorStats actorStats)
        {
            _isInCombat = true;
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetCombat(_isInCombat);

            StartCoroutine(ShootRoutine(actorStats));
        }

        public void StopCombat()
        {
            _isInCombat = false;
            
            if(_aiBehaviourController.ShootingEnemyAnimator != null)
                _aiBehaviourController.ShootingEnemyAnimator.SetCombat(_isInCombat);
        }
    }
}