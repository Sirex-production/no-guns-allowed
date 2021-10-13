using UnityEngine;

namespace Ingame.AI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Ai data", fileName = "NewAiData")]
    public class AiData : ScriptableObject
    {
        [SerializeField] private EnemyType enemyType;
        [Space]
        [SerializeField] private Projectile projectilePrefab;

        public EnemyType EnemyType => enemyType;
        
        public Projectile ProjectilePrefab => projectilePrefab;
    }
}