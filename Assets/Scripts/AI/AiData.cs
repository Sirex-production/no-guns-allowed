using UnityEngine;

namespace Ingame.AI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Ai data", fileName = "NewAiData")]
    public class AiData : ScriptableObject
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] [Min(0)] private float initialHp = 1; 

        public EnemyType EnemyType => enemyType;
        public float InitialHp => initialHp;
    }
}