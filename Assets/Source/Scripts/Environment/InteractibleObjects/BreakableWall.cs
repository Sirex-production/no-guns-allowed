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
            if(other.TryGetComponent(out HitBox hitbox) && hitbox.AttachedActorStats is PlayerStatsController)
                _effectsManager.PlayAllEffects(EffectType.Destruction);
            
            Destroy(gameObject);
        }
    }
}


