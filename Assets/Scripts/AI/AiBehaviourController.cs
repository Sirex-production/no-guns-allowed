using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiMovementController), typeof(AiPatrolController))]
    public class AiBehaviourController : MonoBehaviour
    {
        [SerializeField] private AiData _aiData;

        private AiMovementController _aiMovementController;
        private AiPatrolController _aiPatrolController;
        private Context _context;

        public AiData AiData => _aiData;
        public AiMovementController AiMovementController => _aiMovementController;
        public AiPatrolController AiPatrolController => _aiPatrolController;
        
        private void Awake()
        {
            _aiMovementController = GetComponent<AiMovementController>();
            _aiPatrolController = GetComponent<AiPatrolController>();
        }

        private void Start()
        {
            switch (_aiData.EnemyType)
            {
                case EnemyType.ShootingEnemy:
                    _context = new ShootingEnemyContext(this);
                    break;
            }
        }

        public void SpotEnemy()
        {
            _context.SpotEnemy();
        }

        public void TakeDamage()
        {
            _context.TakeDamage();
        }

        public void EnterRest()
        {
            _context.EnterRest();
        }
    }

    public enum EnemyType
    {
        ShootingEnemy
    }
}