using System.Collections;
using Ingame.UI;
using Support;
using UnityEngine;

namespace Ingame
{
    public class PlayerStatsController : ActorStats
    {
        [SerializeField] private PlayerData data;

        private const int NUMBER_OF_REGENERATED_CHARGES_PER_TICK = 1;
        private const int CHARGES_USED_TO_PERFORM_DASH = 1;

        private float _currentHp;
        private bool _isAlive = true;
        private int _currentNumberOfCharges;

        public PlayerData Data => data;

        public override bool IsInvincible => PlayerEventController.Instance.MovementController.IsDashing;
        public override float CurrentHp => _currentHp;
        public int CurrentNumberOfCharges => _currentNumberOfCharges;
        public bool IsAbleToDash => _isAlive && _currentNumberOfCharges > 0 || !Data.AreChargesUsed;
        
        
        private void Awake()
        {
            _currentNumberOfCharges = data.InitialNumberOfCharges;
        }

        private void Start()
        {
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            
            StartCoroutine(RegenerateChargesRoutine());
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
        }

        private void OnDashPerformed(Vector3 _)
        {
            UseCharges(CHARGES_USED_TO_PERFORM_DASH);
        }
        
        private void Die()
        {
            GameController.Instance.EndLevel(false);
        }

        private IEnumerator RegenerateChargesRoutine()
        {
            while (_isAlive)
            {
                yield return new WaitForSeconds(data.ChargeRegenerationTime);       
                
                RegenerateDashingCharge(NUMBER_OF_REGENERATED_CHARGES_PER_TICK);
            }
        }

        public void RegenerateDashingCharge(int numberOfChargesToRegenerate)
        {
            numberOfChargesToRegenerate = Mathf.Abs(numberOfChargesToRegenerate);
            
            _currentNumberOfCharges += numberOfChargesToRegenerate;
            _currentNumberOfCharges = Mathf.Min(_currentNumberOfCharges, data.InitialNumberOfCharges);
            
            UiController.Instance.UiDashesController.SetNumberOfActiveCharges(_currentNumberOfCharges);
        }

        public void UseCharges(int numberOfChargesToUse)
        {
            if(_currentNumberOfCharges < 1 || !data.AreChargesUsed)
                return;
            
            numberOfChargesToUse = Mathf.Abs(numberOfChargesToUse);
            _currentNumberOfCharges = Mathf.Max(0, _currentNumberOfCharges - numberOfChargesToUse);
            
            UiController.Instance.UiDashesController.SetNumberOfActiveCharges(_currentNumberOfCharges);
        }

        public override void TakeDamage(float amountOfDamage)
        {
            amountOfDamage = Mathf.Abs(amountOfDamage);

            _currentHp -= amountOfDamage;
            
            if(_currentHp < 0)
                Die();
        }

        public override void Heal(float amountOfHp)
        {
            amountOfHp = Mathf.Abs(amountOfHp);

            _currentHp = Mathf.Min(_currentHp + amountOfHp, data.InitialHp);
        }
    }
}