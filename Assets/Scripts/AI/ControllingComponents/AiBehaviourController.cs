using Ingame.Graphics;
using UnityEngine;

namespace Ingame.AI
{
    public class AiBehaviourController : MonoBehaviour
    {
        [SerializeField] private AiData _aiData;

        private IMovable _aiMovementController;
        private IPatrolable _aiPatrolController;
        private ICombatable _aiCombatController;
        private ActorStats _aiActorStats;
        private EffectsManager _effectsManager;
        private Context _context;

        public AiData AiData => _aiData;
        public IMovable AiMovementController => _aiMovementController;
        public IPatrolable AiPatrolController => _aiPatrolController;
        public ICombatable AiCombatController => _aiCombatController;
        public ActorStats AiActorStats => _aiActorStats;
        public EffectsManager EffectsManager => _effectsManager;
        
        private void Awake()
        {
            _aiMovementController = GetComponent<IMovable>();
            _aiPatrolController = GetComponent<IPatrolable>();
            _aiCombatController = GetComponent<ICombatable>();
            _aiActorStats = GetComponent<ActorStats>();
            _effectsManager = GetComponent<EffectsManager>();
        }

        private void Start()
        {
            switch (_aiData.EnemyType)
            {
                case EnemyType.ShootingEnemy:
                    _context = new Context(this, new ShootingEnemyRestStage());
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

        public void Die()
        {
            _context.Die();
        }

        public void DestroyActor()
        {
            Destroy(gameObject);
        }
    }

    public enum EnemyType
    {
        ShootingEnemy
    }
}