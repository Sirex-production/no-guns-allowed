using DG.Tweening;
using Ingame.Sound;
using Support.Extensions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.Graphics
{
    public class IconPopupEffect : Effect
    {
        [Required][SerializeField] private Image detectionImage;
        [SerializeField] private Color blinkColor = Color.red;
        [Foldout("Animation settings")] [SerializeField] [Min(-1)] private int amountOfBlinks = -1;
        [Foldout("Animation settings")] [SerializeField] [Min(0)] private float blinkingTime = .3f;
        [Space] 
        [SerializeField] private bool alwaysLookAtCamera = true;
        [Space] 
        [SerializeField] private AudioName audioNameToPlayOnPopup = AudioName.none;

        [Inject] private readonly LegacyAudioManager _legacyAudioManager;
        
        private Sequence _imageAnimationSequence;

        protected override void Start()
        {
            var mainCamera = Camera.main;

            if(mainCamera == null)
                return;
            
            var canvas = detectionImage.canvas;
            canvas.worldCamera = Camera.main;
            canvas.transform.LookAt(mainCamera.transform);

            if (alwaysLookAtCamera)
            {
                if (Camera.main != null)
                {
                    var mainCameraTransform = Camera.main.transform;
                    this.RepeatCoroutine(0, () => transform.LookAt(mainCameraTransform));
                }
            }

            base.Start();
        }

        public override void PlayEffect(Transform instanceTargetTransform)
        {
            if(instanceTargetTransform != null)
                transform.parent = instanceTargetTransform;
            
            _legacyAudioManager.PlaySound(audioNameToPlayOnPopup);
            
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