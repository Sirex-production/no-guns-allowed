using Ingame;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StickyWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out PlayerMovementController playerMovementController))
            playerMovementController.FreezePlayer();
    }
}
