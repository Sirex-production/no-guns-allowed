using System;
using Extensions;
using Ingame.AI;
using UnityEngine;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(EffectsManager))]
    public class BreakableWall : MonoBehaviour
    {
        private EffectsManager _effectsManager;

        private void Awake()
        {
            _effectsManager = GetComponent<EffectsManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HitBox hitbox) && hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController)
            {
                if (PlayerEventController.Instance.StatsController.IsInvincible)
                {
                    _effectsManager.PlayAllEffects(EffectType.Destruction);
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

        private void OnDashPerformed(Vector3 _)
        {
            _effectsManager.PlayAllEffects(EffectType.Destruction);
            Destroy(gameObject);
        }
    }
}


