using UnityEngine;

namespace Ingame
{
    public interface Projectile
    {
        public void Launch(Transform destination, params ActorStats[] ignoreHitActors);
        public void Launch(Vector3 direction, params ActorStats[] ignoreHitActors);
        public void Reflect(Vector3 direction);
    }
}