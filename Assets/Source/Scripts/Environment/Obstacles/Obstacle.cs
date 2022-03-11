using System.Linq;
using Extensions;
using Ingame.AI;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [Required][SerializeField] private ObstacleData obstacleData;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HitBox hitBox))
            {
                if(obstacleData.FriendlySides.Contains(hitBox.AttachedActorStats.ActorSide))
                    return;
                
                hitBox.TakeDamage(obstacleData.Damage, DamageType.Environment);
            }
        }
    }
}