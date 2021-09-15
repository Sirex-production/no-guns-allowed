using System.Collections;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(PlayerMovementController), typeof(PlayerAnimationController))]
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private PlayerData data;

        private const int NUMBER_OF_REGENERATED_CHARGES_PER_TICK = 1;
        
        private bool _isAlive = true;
        private int _currentNumberOfCharges;
        
        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;

        public PlayerData Data => data;

        public int CurrentNumberOfCharges => _currentNumberOfCharges;
        public bool IsAbleToDash => _isAlive && _currentNumberOfCharges > 0;
        
        public PlayerMovementController MovementController => _movementController;
        public PlayerAnimationController AnimationController => _animationController;

        private void Awake()
        {
            _currentNumberOfCharges = data.InitialNumberOfCharges;
            
            _movementController = GetComponent<PlayerMovementController>();
            _animationController = GetComponent<PlayerAnimationController>();
        }

        private void Start()
        {
            StartCoroutine(RegenerateChargesRoutine());
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
            
            UiController.Instance.UiDashesController.SetNumberOfCharges(_currentNumberOfCharges, data.InitialNumberOfCharges);
        }

        public void UseCharges(int numberOfChargesToUse)
        {
            if(_currentNumberOfCharges < 1)
                return;
            
            numberOfChargesToUse = Mathf.Abs(numberOfChargesToUse);
            _currentNumberOfCharges = Mathf.Max(0, _currentNumberOfCharges - numberOfChargesToUse);
            
            UiController.Instance.UiDashesController.SetNumberOfCharges(_currentNumberOfCharges, data.InitialNumberOfCharges);
        }
    }
}