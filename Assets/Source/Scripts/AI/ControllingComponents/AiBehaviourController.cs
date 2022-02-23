using Ingame.Graphics;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.AI
{
    [DisallowMultipleComponent]
    public class AiBehaviourController : MonoBehaviour
    {
        [Required]
        [SerializeField] private AiData _aiData;

        [Inject] private AnalyticsWrapper _analyticsWrapper;
        [Inject] private ActorManager _actorManager;
        
        private IMovable _aiMovementController;
        private IPatrolable _aiPatrolController;
        private ICombatable _aiCombatController;
        private ActorStats _aiActorStats;
        private EffectsFactory _effectsFactory;
        private Context _context;

        public AiData AiData => _aiData;
        public IMovable AiMovementController => _aiMovementController;
        public IPatrolable AiPatrolController => _aiPatrolController;
        public ICombatable AiCombatController => _aiCombatController;
        public ActorStats AiActorStats => _aiActorStats;
        public EffectsFactory EffectsFactory => _effectsFactory;
        
        private void Awake()
        {
            _aiMovementController = GetComponent<IMovable>();
            _aiPatrolController = GetComponent<IPatrolable>();
            _aiCombatController = GetComponent<ICombatable>();
            _aiActorStats = GetComponent<ActorStats>();
            _effectsFactory = GetComponent<EffectsFactory>();
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
            _actorManager.RemoveActor(_aiActorStats);
            _analyticsWrapper.LevelStats.AddKilledEnemyToStats();
            _context.Die();
        }

        public void DestroyActor()
        {
            if(EffectsFactory != null)
               EffectsFactory.PlayAllEffects(EffectType.EnemyDeath);
            
            Destroy(gameObject);
        }
    }

    public enum EnemyType
    {
        ShootingEnemy
    }
}