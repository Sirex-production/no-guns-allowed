using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class InvokableDoor : Invokable
    {
        [SerializeField] [Min(0)] [MaxValue(90)] private float rotationAngle;
        [SerializeField] [Min(0)]                private float duration;
        [SerializeField]                         private bool  isClockwise;

        public override void Invoke()
        {
            OpenDoor();
        }


        private void OpenDoor()
        {
            var direction = isClockwise ? -1 : 1;
            var tweenEndValue = new Vector3(0, 0, rotationAngle * direction);

            transform.DORotate(tweenEndValue, duration, RotateMode.LocalAxisAdd);
        }
    }
}