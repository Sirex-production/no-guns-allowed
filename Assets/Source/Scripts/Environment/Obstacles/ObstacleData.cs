using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(menuName = "Data/Ingame/Obstacle data", fileName = "NewObstacleData")]
    public class ObstacleData : ScriptableObject
    {
        [SerializeField] private ActorSide[] friendlySides;
        [SerializeField] [Min(0)] private float damage;

        public ActorSide[] FriendlySides => friendlySides;
        public float Damage => damage;
    }
}