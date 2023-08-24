using Ingame.Graphics;
using NaughtyAttributes;
using Support.Extensions;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class PlayerEffects : MonoBehaviour
    {
        [BoxGroup("Sword strike effect"), Required] 
        [SerializeField] private Animator swordStrikeAnimator;
        [BoxGroup("Sword strike effect")]
        [SerializeField] private float swordStrikeEffectDuration = .02f;
        
        [Inject] private EffectsManager _effectsManager;

        private void Awake()
        {
            swordStrikeAnimator.SetGameObjectInactive();
        }

        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += RotateSwordStrikeEffectTowardsDashDirection;
            _effectsManager.OnPlayerAttackEffectPlayed += PlaySwordStrikeEffect;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= RotateSwordStrikeEffectTowardsDashDirection;
            _effectsManager.OnPlayerAttackEffectPlayed -= PlaySwordStrikeEffect;
        }

        private void PlaySwordStrikeEffect()
        {
            swordStrikeAnimator.SetGameObjectActive();
            swordStrikeAnimator.SetTrigger("Strike");
            this.DoAfterNextFrameCoroutine(() => swordStrikeAnimator.ResetTrigger("Strike"));
            this.WaitAndDoCoroutine(swordStrikeEffectDuration, () =>
            {
                swordStrikeAnimator.SetGameObjectInactive();
            });
        }

        private void RotateSwordStrikeEffectTowardsDashDirection(Vector3 dashDirection)
        {
            if(dashDirection.sqrMagnitude < .00001f )
                return;
            
            swordStrikeAnimator.transform.rotation = Quaternion.LookRotation(dashDirection);
        }
    }
}