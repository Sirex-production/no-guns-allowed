using Ingame.Graphics;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.AI
{
    [DisallowMultipleComponent]
    public class AiBehaviourController : MonoBehaviour
    {
        [Required]
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
                    _context = new Context(this);
                    _context.CurrentState = new ShootingEnemyRestStage(_context);
                    break;
            }
        }

        public void SpotEnemy(ActorStats actorStats)
        {
            _context.SpotEnemy(actorStats);
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
            if(EffectsManager != null)
               EffectsManager.PlayAllEffects(EffectType.Destruction);
            
            Destroy(gameObject);
        }
    }

    public enum EnemyType
    {
        ShootingEnemy
    }
}