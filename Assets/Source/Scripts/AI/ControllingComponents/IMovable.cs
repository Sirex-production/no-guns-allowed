using System;
using UnityEngine;

namespace Ingame.AI
{
    public interface IMovable
    {
        public void Follow(Transform transformToFollow, float speed, Action onEnd = null);
        public void MoveTo(Vector3 destination, float speed, Action onEnd = null);
        public void Rotate(Quaternion rotation, float speed, Action onEnd = null);
        public void StopMotion();
    }
}