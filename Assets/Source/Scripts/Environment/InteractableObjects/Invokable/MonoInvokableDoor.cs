using DG.Tweening;
using UnityEngine;

namespace Ingame
{
    public class MonoInvokableDoor : MonoInvokable
    {
        [SerializeField] [Range(-180, 180)] private float rotationAngle;
        [SerializeField] [Min(0)]                private float duration;
        [SerializeField]                         private bool  isClockwise;

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
            OpenDoor();
        }


        private void OpenDoor()
        {
            transform.DOLocalRotate(_isOpened ? _initialLocalRotation : _targetAngle, duration);

            _isOpened = !_isOpened;
        }
    }
}