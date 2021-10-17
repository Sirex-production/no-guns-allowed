using UnityEngine;

namespace Ingame
{
    public abstract class ActorStats : MonoBehaviour
    {
        public abstract float CurrentHp { get; }
        public abstract bool IsInvincible { get; }
        public abstract void TakeDamage(float amountOfDamage);
        public abstract void Heal(float amountOfHp);
    }
}