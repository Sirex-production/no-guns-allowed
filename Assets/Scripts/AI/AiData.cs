using UnityEngine;

namespace Ingame.AI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Ai data", fileName = "NewAiData")]
    public class AiData : ScriptableObject
    {
        [SerializeField] private EnemyType enemyType;

        public EnemyType EnemyType => enemyType;
    }
}