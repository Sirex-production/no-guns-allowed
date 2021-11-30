using Ingame.Graphics;

namespace Ingame.AI
{
    public class ShootingEnemyRestStage : State
    {
        private const float CHANCE_TO_STOP_PATROLLING= 40;
        
        public ShootingEnemyRestStage(Context context) : base(context)
        {
            _currentContext.AiBehaviourController.AiPatrolController.StartPatrolling();
        }

        public override State HandleSpotEnemy(ActorStats actorStats)
        {
            //todo play spot animation
            //if(Random.Range(0, 100) <= CHANCE_TO_STOP_PATROLLING)
            //    currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            
            _currentContext.AiBehaviourController.EffectsManager.PlayAllEffects(EffectType.Detection);
            
            return new ShootingEnemyCombatState(actorStats, _currentContext);
        }

        public override State HandleTakeDamage()
        {
            return this;
        }

        public override State HandleEnterRest()
        {
            return this;
        }

        public override State HandleDeath()
        {
            _currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            _currentContext.AiBehaviourController.DestroyActor();

            return this;
        }
    }
}