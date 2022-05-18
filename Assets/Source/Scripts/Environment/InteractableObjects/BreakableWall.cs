using Ingame.AI;
using Support.Sound;
using UnityEngine;
using Zenject;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(EffectsFactory))]
    public class BreakableWall : MonoBehaviour
    {
        [Inject] private EffectsManager _effectsManager;
        [Inject] private AudioManager _audioManager;
        
        private const float MIN_COS_OF_ANGLE_TO_BREAK_THE_WALL = .75f;
        
        private EffectsFactory _effectsFactory;

        private void Awake()
        {
            _effectsFactory = GetComponent<EffectsFactory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collider outCollider) && outCollider.CompareTag("EnvironmentalHitBox"))
            {
                if (PlayerEventController.Instance.StatsController.IsInvincible)
                {
                    _audioManager.PlayRandomizedSound(false,
                        AudioName.environment_wall_destruction_1, 
                        AudioName.environment_wall_destruction_2,
                        AudioName.environment_wall_destruction_3,
                        AudioName.environment_wall_destruction_4);
                    
                    _effectsFactory.PlayAllEffects(EffectType.Destruction);
                    _effectsManager.PlayPlayerAttackEffect();
                    Destroy(gameObject);
                    
                    return;
                }
                
                PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out HitBox hitbox) && hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController)
            {
                PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
            }
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        private void OnDashPerformed(Vector3 dashDirection)
        {
            
            var directionToTheWall = Vector3.Normalize(transform.position - PlayerEventController.Instance.transform.position);
            var dotProductBetweenDashAndWallDirection = Vector3.Dot(directionToTheWall, dashDirection);


            if (dotProductBetweenDashAndWallDirection < MIN_COS_OF_ANGLE_TO_BREAK_THE_WALL)
                return;

            _audioManager.PlayRandomizedSound(false,
                AudioName.environment_wall_destruction_1, 
                AudioName.environment_wall_destruction_2,
                AudioName.environment_wall_destruction_3,
                AudioName.environment_wall_destruction_4);
            
            _effectsFactory.PlayAllEffects(EffectType.Destruction);
            _effectsManager.PlayPlayerAttackEffect();
            Destroy(gameObject);
        }
    }
}


