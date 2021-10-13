using UnityEngine;

namespace Ingame
{
    public interface Projectile
    {
        public void Launch(Transform destination);
        public void Launch(Vector3 direction);
    }
}