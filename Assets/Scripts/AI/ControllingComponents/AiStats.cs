using UnityEngine;

namespace Ingame.AI
{
    public class AiStats : ActorStats
    {
        [SerializeField] private float initialHp;

        private float _currentHp;

        public override float CurrentHp => _currentHp;
        public override bool IsInvincible => false;
        
        private void Awake()
        {
            _currentHp = initialHp;
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public override void TakeDamage(float amountOfDamage)
        {
            amountOfDamage = Mathf.Abs(amountOfDamage);

            _currentHp -= amountOfDamage;
            
            if(_currentHp < 1)
                Die();
        }

        public override void Heal(float amountOfHp)
        {
            amountOfHp = Mathf.Abs(amountOfHp);

            _currentHp += amountOfHp;
            _currentHp = Mathf.Min(initialHp, _currentHp);
        }
    }
}