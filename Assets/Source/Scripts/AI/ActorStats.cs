using Ingame.AI;
using UnityEngine;

namespace Ingame
{
    public abstract class ActorStats : MonoBehaviour
    {
        protected virtual void Start()
        {
            ActorManager.Instance.AddEnemy(this);
        }

        public abstract ActorSide ActorSide { get; }
        public abstract float CurrentHp { get; }
        public abstract float CurrentDamage { get; }
        public abstract bool IsInvincible { get; }
        public abstract void TakeDamage(float amountOfDamage);
        public abstract void Heal(float amountOfHp);
    }

    public enum ActorSide
    {
        Enemy, Alie, Player, Neutral
    }
}