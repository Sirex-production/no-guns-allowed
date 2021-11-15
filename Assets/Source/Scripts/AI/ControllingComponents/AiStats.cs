using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    [DisallowMultipleComponent]
    public class AiStats : ActorStats
    {
        private float _currentHp;
        private AiBehaviourController _aiBehaviourController;
        
        public override float CurrentHp => _currentHp;
        public override float CurrentDamage => _aiBehaviourController.AiData.MeleeDamage;
        public override bool IsInvincible => false;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
            _currentHp = _aiBehaviourController.AiData.InitialHp;
        }
        
        public override void TakeDamage(float amountOfDamage)
        {
            if(IsInvincible || _currentHp < 1)
                return;
            
            amountOfDamage = Mathf.Abs(amountOfDamage);

            _currentHp -= amountOfDamage;
            
            if(_currentHp < 1)
                _aiBehaviourController.Die();
        }

        public override void Heal(float amountOfHp)
        {
            amountOfHp = Mathf.Abs(amountOfHp);

            _currentHp += amountOfHp;
            _currentHp = Mathf.Min(_aiBehaviourController.AiData.InitialHp, _currentHp);
        }
    }
}