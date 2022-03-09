using UnityEngine;

namespace Ingame
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract void Launch(Transform destination, params ActorStats[] ignoreHitActors);
        public abstract void Launch(Vector3 direction, params ActorStats[] ignoreHitActors);
    }
}