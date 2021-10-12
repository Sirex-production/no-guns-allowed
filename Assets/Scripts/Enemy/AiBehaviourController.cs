using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiMovementController), typeof(AiPatrolController))]
    public class AiBehaviourController : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        private AiMovementController _aiMovementController;
        private AiPatrolController _aiPatrolController;
        private Context _context;

        public AiMovementController AiMovementController => _aiMovementController;
        public AiPatrolController AiPatrolController => _aiPatrolController;
        
        private void Awake()
        {
            _aiMovementController = GetComponent<AiMovementController>();
            _aiPatrolController = GetComponent<AiPatrolController>();
        }

        private void Start()
        {
            switch (enemyType)
            {
                case EnemyType.ShootingEnemy:
                    _context = new ShootingEnemyContext(this);
                    break;
            }
        }
    }

    public enum EnemyType
    {
        ShootingEnemy
    }
}