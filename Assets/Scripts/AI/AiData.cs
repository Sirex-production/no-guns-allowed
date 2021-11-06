using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Ai data", fileName = "NewAiData")]
    public class AiData : ScriptableObject
    {
        [Foldout("Behaviour")]
        [SerializeField] private EnemyType enemyType;
        
        [Foldout("Stats")]
        [SerializeField] [Min(0)] private float initialHp = 1;
        
        [Foldout("Movement")] 
        [SerializeField] private bool useGravity;
        
        [Foldout("Patrol")] 
        [SerializeField] private bool isLooped;
        [Foldout("Patrol")] 
        [SerializeField] [Min(0)] private float speed = 10f;
        
        [Foldout("Combat"), Required()]
        [SerializeField] private Bullet bulletPrefab;
        [Foldout("Combat"), Tooltip("Controls whether AI will shoot if the opponent cannot be seen directly or not")]
        [SerializeField] private bool ignoreBarriers = true;
        [Foldout("Combat")]
        [SerializeField] [Min(0)] private float meleeDamage = 5f;
        [Foldout("Combat")]
        [SerializeField] [Min(0)] private float pauseBetweenShots = 1;
        
        [Foldout("Sight")]
        [SerializeField] private bool onlyPlayerCanBeDetected = true;

        public EnemyType EnemyType => enemyType;
        
        public float InitialHp => initialHp;

        public bool UseGravity => useGravity;

        public bool IsLooped => isLooped;
        public float Speed => speed;

        public Bullet BulletPrefab => bulletPrefab;
        public bool IgnoreBarriers => ignoreBarriers;
        public float MeleeDamage => meleeDamage;
        public float PauseBetweenShots => pauseBetweenShots;

        public bool OnlyPlayerCanBeDetected => onlyPlayerCanBeDetected;
    }
}