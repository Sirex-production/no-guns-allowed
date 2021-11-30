using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Graphics
{
    public class BreakableWall : MonoBehaviour
    {
        private EffectsManager _effectsManager;

        private void Awake()
        {
            _effectsManager = GetComponent<EffectsManager>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(_effectsManager != null)
                _effectsManager.PlayAllEffects(EffectType.Destruction);
            
            Destroy(gameObject);
        }
    }
}


