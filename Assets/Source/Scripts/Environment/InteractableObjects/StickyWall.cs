using Ingame;
using Ingame.AI;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StickyWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox)) 
            return;

        if(!hitbox.AttachedActorStats.TryGetComponent(out PlayerMovementController controller))
            return;

        controller.FreezePlayer();
    }
}
