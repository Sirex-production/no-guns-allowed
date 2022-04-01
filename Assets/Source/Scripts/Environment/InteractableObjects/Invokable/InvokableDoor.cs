using DG.Tweening;
using Extensions;
using Ingame.Graphics;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class InvokableDoor : MonoInvokable
    {
        [SerializeField] [Range(-180, 180)] private float rotationAngle;
        [SerializeField] [Min(0)] private float duration;
        [SerializeField] [Min(0)] private float delayBeforeRotation = 0;
        [SerializeField] private bool isClockwise;
        [SerializeField] private bool shakeEnvironment = false;

        [Inject] private EffectsManager _effectsManager;
        
        private bool _isOpened;
        private Vector3 _initialLocalRotation;
        private Vector3 _targetAngle;

        private void Awake()
        {
            _initialLocalRotation = transform.localEulerAngles;
            _targetAngle = _initialLocalRotation + Vector3.forward * rotationAngle * (isClockwise ? -1 : 1);
        }

        public override void Invoke()
        {
            this.WaitAndDoCoroutine(delayBeforeRotation, OpenDoor);
        }
        
        private void OpenDoor()
        {
            transform.DOLocalRotate(_isOpened ? _initialLocalRotation : _targetAngle, duration);

            _isOpened = !_isOpened;
            
            if(shakeEnvironment)
                _effectsManager.ShakeEnvironment(duration);
        }
    }
}