using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Ai data", fileName = "NewAiData")]
    public class AiData : ScriptableObject
    {
        [BoxGroup("Behaviour")]
        [SerializeField] private EnemyType enemyType;
        [BoxGroup("Behaviour")] 
        [SerializeField] private ActorSide actorSide = ActorSide.Enemy;
        [BoxGroup("Behaviour"), Tooltip("Sides that given AI will treat as enemy")]
        [SerializeField] private List<ActorSide> hostileSides;
        
        [BoxGroup("Stats")]
        [SerializeField] [Min(0)] private float initialHp = 1;
        
        [BoxGroup("Movement")] 
        [SerializeField] private bool useGravity;
        
        [BoxGroup("Patrol")] 
        [SerializeField] private bool isLooped;
        [BoxGroup("Patrol")] 
        [SerializeField] [Min(0)] private float speed = 10f;
        
        [BoxGroup("Combat"), Required]
        [SerializeField] private Bullet bulletPrefab;
        [BoxGroup("Combat"), Tooltip("Controls whether AI will shoot if the opponent cannot be seen directly or not")]
        [SerializeField] private bool ignoreBarriers = true;
        [BoxGroup("Combat")]
        [SerializeField] [Min(0)] private float meleeDamage = 5f;
        [BoxGroup("Combat")]
        [SerializeField] [Min(0)] private float pauseBetweenShots = 1;

        public EnemyType EnemyType => enemyType;
        public ActorSide ActorSide => actorSide;
        public List<ActorSide> HostileSides => hostileSides;
        
        public float InitialHp => initialHp;

        public bool UseGravity => useGravity;

        public bool IsLooped => isLooped;
        public float Speed => speed;

        public Bullet BulletPrefab => bulletPrefab;
        public bool IgnoreBarriers => ignoreBarriers;
        public float MeleeDamage => meleeDamage;
        public float PauseBetweenShots => pauseBetweenShots;
    }
}