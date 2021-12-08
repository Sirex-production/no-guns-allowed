using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.Graphics
{
    public class DetectionEffect : Effect
    {
        [Required][SerializeField] private Image detectionImage;
        [SerializeField] private Color blinkColor = Color.red;
        [Foldout("Animation settings")] [SerializeField] [Min(-1)] private int amountOfBlinks = -1;
        [Foldout("Animation settings")] [SerializeField] [Min(0)] private float blinkingTime = .3f;
        
        private Sequence _imageAnimationSequence;

        private void Start()
        {
            if(Camera.main != null)
                return;
            
            var canvas = detectionImage.canvas;
            canvas.worldCamera = Camera.main;
            canvas.transform.LookAt(Camera.main.transform);
        }

        public override void PlayEffect(Transform instanceTargetTransform)
        {
            if(instanceTargetTransform != null)
                transform.parent = instanceTargetTransform;
            
            var initialColor = detectionImage.color;

            _imageAnimationSequence = DOTween.Sequence()
                .Append(detectionImage.DOColor(blinkColor, blinkingTime))
                .Append(detectionImage.DOColor(initialColor, blinkingTime))
                .SetLoops(amountOfBlinks);
        }

        private void OnDestroy()
        {
            if(_imageAnimationSequence != null)
                _imageAnimationSequence.Kill();
        }
    }
}