using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Graphics
{
    public class EffectsManager : MonoBehaviour
    {
        [Space]
        [SerializeField] private List<EffectEntity> effectEntities;

        private const float GIZMOS_CUBE_SIZE = .3f; 
        
        private void OnDrawGizmos()
        {
            if(effectEntities == null || effectEntities.Count < 1)
                return;
            
            Gizmos.color = Color.yellow;
            
            foreach (var effectEntity in effectEntities)
            {
                if(effectEntity == null)
                    continue;
                
                Gizmos.DrawCube(effectEntity.effectTransform.position, Vector3.one * GIZMOS_CUBE_SIZE);
                Gizmos.DrawLine(transform.position, effectEntity.effectTransform.position);
            }
        }

        public virtual void PlayAllEffects(EffectType effectType)
        {
            if(effectEntities == null || effectEntities.Count < 1)
                return;

            foreach (var effectEntity in effectEntities)
            {
                if(effectEntity == null || effectEntity.effectPrefab == null)
                    continue;
                
                if(!effectEntity.effectPrefab.EffectTypes.Contains(effectType))
                    continue;

                var effect = Instantiate(effectEntity.effectPrefab, effectEntity.effectTransform.position, effectEntity.effectTransform.rotation);
                effect.PlayEffect(effectEntity.effectTransform);
            }
        }
    }

    [Serializable]
    public class EffectEntity
    {
        public Effect effectPrefab;
        public Transform effectTransform;
    }
}