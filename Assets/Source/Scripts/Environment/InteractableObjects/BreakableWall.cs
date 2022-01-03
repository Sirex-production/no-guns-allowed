using System;
using Extensions;
using Ingame.AI;
using UnityEngine;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(EffectsFactory))]
    public class BreakableWall : MonoBehaviour
    {
        private EffectsFactory _effectsFactory;

        private void Awake()
        {
            _effectsFactory = GetComponent<EffectsFactory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HitBox hitbox) && hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController)
            {
                if (PlayerEventController.Instance.StatsController.IsInvincible)
                {
                    _effectsFactory.PlayAllEffects(EffectType.Destruction);
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
            _effectsFactory.PlayAllEffects(EffectType.Destruction);
            Destroy(gameObject);
        }
    }
}


