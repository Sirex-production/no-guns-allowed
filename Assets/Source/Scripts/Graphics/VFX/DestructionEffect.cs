using System.Collections;
using System.Collections.Generic;
using Extensions;
using Ingame.Graphics;
using UnityEngine;

namespace Ingame.Graphics
{
    public class DestructionEffect : Effect
    {
        public override void PlayEffect(Transform instanceTargetTransform)
        {
            transform.localScale = instanceTargetTransform.transform.localScale;


        }
    }
}

