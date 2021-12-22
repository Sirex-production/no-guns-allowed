using DG.Tweening;
using Ingame.AI;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class InteractiveDoor : MonoBehaviour
    {
        [SerializeField] [Min(0)] [MaxValue(90)] private float rotationAngle;
        [SerializeField] [Min(0)]                private float duration;

        private bool _hasBeenInvoked;

        //TODO: Destroy collider after interaction (?)
        //TODO: Find a way to implement this as an Effect
        private void OnTriggerEnter(Collider other)
        {
            if(_hasBeenInvoked)
                return;

            if (other.TryGetComponent(out HitBox hitbox) && hitbox.AttachedActorStats == PlayerEventController.Instance.StatsController)
            {
                _hasBeenInvoked = true;
                OpenDoor();
            }
        }

        private void OpenDoor()
        {
            var directionVector = Vector3.Normalize(PlayerEventController.Instance.transform.position - transform.position);
            var directionNormalized = directionVector.x;

            var direction = (int)(directionNormalized / Mathf.Abs(directionNormalized));
            var tweenEndValue = new Vector3(rotationAngle * direction, 0, 0);

            transform.DORotate(tweenEndValue, duration, RotateMode.LocalAxisAdd);
        }
    }
}
