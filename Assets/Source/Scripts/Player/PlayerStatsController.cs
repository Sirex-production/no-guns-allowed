using System.Collections;
using Extensions;
using Ingame.AI;
using Ingame.Graphics;
using Ingame.UI;
using MoreMountains.NiceVibrations;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(PlayerEventController))]
    public class PlayerStatsController : ActorStats
    {
        [SerializeField] private PlayerData data;
        
        [Inject] private AnalyticsWrapper _analyticsWrapper;
        [Inject] private UiController _uiController;
        [Inject] private EffectsManager _effectsManager;
        
        private const int NUMBER_OF_REGENERATED_CHARGES_PER_TICK = 1;
        private const int CHARGES_USED_TO_PERFORM_DASH = 1;

        private const float TIME_AFTER_DASH_WHEN_PLAYER_IS_INVINCIBLE = .2f;
        
        private float _currentHp;
        private bool _isAlive = true;
        private bool _isInvincible = false;
        private int _chargeRegenerationTimeModifier;
        private int _currentNumberOfCharges;

        public PlayerData Data => data;

        public override ActorSide ActorSide => ActorSide.Player;
        public override float CurrentDamage => data.Damage;
        public override bool IsInvincible => PlayerEventController.Instance.MovementController.IsDashing || _isInvincible;
        public override float CurrentHp => _currentHp;
        public int CurrentNumberOfCharges => _currentNumberOfCharges;
        public bool IsAbleToDash => _isAlive && _currentNumberOfCharges > 0 || !Data.AreChargesUsed;
        public int ChargeRegenerationTimeModifier { get => _chargeRegenerationTimeModifier; set => _chargeRegenerationTimeModifier = value; }
        public bool IsFullyCharged => _currentNumberOfCharges >= data.InitialNumberOfCharges;


        private void Awake()
        {
            _currentNumberOfCharges = data.InitialNumberOfCharges;
            _chargeRegenerationTimeModifier = 1;
        }

        protected override void Start()
        {
            base.Start();
            
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            
            StartCoroutine(RegenerateChargesRoutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out HitBox actorStats) && IsInvincible)
            {
                actorStats.TakeDamage(data.Damage, DamageType.PlayerMelee);
                VibrationController.Vibrate(HapticTypes.RigidImpact);
                _effectsManager.PlayPlayerAttackEffect();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out HitBox actorStats) && IsInvincible)
            {
                actorStats.TakeDamage(data.Damage, DamageType.PlayerMelee);
                VibrationController.Vibrate(HapticTypes.RigidImpact);
                _effectsManager.PlayPlayerAttackEffect();
            }
        }
        
        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        private void OnDashPerformed(Vector3 _)
        {
            UseCharges(CHARGES_USED_TO_PERFORM_DASH);
        }
        
        private void Die(DamageType damageType)
        {
            if(_analyticsWrapper != null)
                _analyticsWrapper.LevelStats.AddPlayerDeathToStats(damageType);
            
            _effectsManager.PlayPlayerDeathEffect();
            Destroy(gameObject);
        }

        private IEnumerator RegenerateChargesRoutine()
        {
            while (_isAlive)
            {
                yield return new WaitForSeconds(data.ChargeRegenerationTime / _chargeRegenerationTimeModifier);       
                
                RegenerateDashingCharge(NUMBER_OF_REGENERATED_CHARGES_PER_TICK);
            }
        }

        public void RegenerateDashingCharge(int numberOfChargesToRegenerate)
        {
            numberOfChargesToRegenerate = Mathf.Abs(numberOfChargesToRegenerate);
            
            _currentNumberOfCharges += numberOfChargesToRegenerate;
            _currentNumberOfCharges = Mathf.Min(_currentNumberOfCharges, data.InitialNumberOfCharges);
            
            _uiController.UiDashesController.SetNumberOfActiveCharges(_currentNumberOfCharges);
        }

        public void UseCharges(int numberOfChargesToUse)
        {
            if(_currentNumberOfCharges < 1 || !data.AreChargesUsed)
                return;

            _isInvincible = true;
            
            numberOfChargesToUse = Mathf.Abs(numberOfChargesToUse);
            _currentNumberOfCharges = Mathf.Max(0, _currentNumberOfCharges - numberOfChargesToUse);
            
            _uiController.UiDashesController.SetNumberOfActiveCharges(_currentNumberOfCharges);
            this.WaitAndDoCoroutine(TIME_AFTER_DASH_WHEN_PLAYER_IS_INVINCIBLE, () => _isInvincible = false);
        }

        public override void TakeDamage(float amountOfDamage, DamageType damageType)
        {
            if(IsInvincible)
                if(damageType != DamageType.Environment && damageType != DamageType.Explosion)
                    return;
            
            amountOfDamage = Mathf.Abs(amountOfDamage);

            _currentHp -= amountOfDamage;
            
            if(_currentHp < 0)
                Die(damageType);
        }

        public override void Heal(float amountOfHp)
        {
            amountOfHp = Mathf.Abs(amountOfHp);

            _currentHp = Mathf.Min(_currentHp + amountOfHp, data.InitialHp);
        }
    }
}