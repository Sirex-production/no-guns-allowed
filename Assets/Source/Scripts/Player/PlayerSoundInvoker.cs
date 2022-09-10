using Ingame.Graphics;
using Ingame.Sound;
using UnityEngine;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(PlayerEventController))]
    public class PlayerSoundInvoker : MonoBehaviour
    {
        [Inject] private readonly LegacyAudioManager _legacyAudioManager;
        [Inject] private readonly EffectsManager _effectsManager;

        private PlayerEventController _playerEventController;
        
        private void Awake()
        {
            _playerEventController = GetComponent<PlayerEventController>();
        }

        private void Start()
        {
            _playerEventController.OnDashPerformed += OnDashPerformed;
            _effectsManager.OnEnemyKillEffectPlayed += OnPlayerAttackEffectPlayed;
            _playerEventController.OnDashCancelled += OnDashCanceled;
        }

        private void OnDestroy()
        {
            _playerEventController.OnDashPerformed -= OnDashPerformed;
            _effectsManager.OnEnemyKillEffectPlayed -= OnPlayerAttackEffectPlayed;
            _playerEventController.OnDashCancelled -= OnDashCanceled;
        }

        private void OnDashCanceled()
        {
            _legacyAudioManager.PlaySound(AudioName.ui_out_of_charges);
        }

        private void OnDashPerformed(Vector3 _)
        {
            _legacyAudioManager.PlaySound(AudioName.player_dash);
        }

        private void OnPlayerAttackEffectPlayed(DamageType damageType)
        {
            if (damageType == DamageType.PlayerMelee)
                _legacyAudioManager.PlayRandomizedSound(false,
                    AudioName.player_sword_swing_1,
                    AudioName.player_sword_swing_2,
                    AudioName.player_sword_swing_3,
                    AudioName.player_sword_swing_4);
        }
    }
}