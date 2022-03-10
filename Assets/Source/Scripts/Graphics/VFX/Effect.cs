using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Ingame.Graphics
{
    public abstract class Effect : MonoBehaviour
    {
        [SerializeField] protected List<EffectType> effectTypes;
        [Space]
        [SerializeField] protected bool isEffectDestroyedAfterSomeTime = true;
        [SerializeField] [Min(0)] protected float timeAfterEffectWillBeDestroyed = 10;

        public List<EffectType> EffectTypes => effectTypes; 

        protected virtual void Start()
        {
            if (isEffectDestroyedAfterSomeTime)
                this.WaitAndDoCoroutine(timeAfterEffectWillBeDestroyed, () => Destroy(gameObject));
        }

        public abstract void PlayEffect(Transform instanceTargetTransform);
    }

    public enum EffectType
    {
        EnemyDeath,
        Detection,
        Destruction,
        EnemyConfuse
    }
}