using Ingame.AI;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public abstract class ActorStats : MonoBehaviour
    {
        [Inject] private ActorManager _actorManager;
        
        protected virtual void Start()
        {
            _actorManager.AddActor(this);
        }

        public abstract ActorSide ActorSide { get; }
        public abstract float CurrentHp { get; }
        public abstract float CurrentDamage { get; }
        public abstract bool IsInvincible { get; }
        public abstract void TakeDamage(float amountOfDamage, DamageType damageType);
        public abstract void Heal(float amountOfHp);
    }

    public enum ActorSide
    {
        Enemy, Alie, Player, Neutral
    }
}