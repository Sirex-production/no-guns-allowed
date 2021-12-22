using System;
using System.Collections;
using System.Collections.Generic;
using Ingame;
using Ingame.AI;
using Ingame.Graphics;
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
