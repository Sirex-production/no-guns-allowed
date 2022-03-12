using Ingame.Graphics;

namespace Ingame.AI
{
    public class ShootingEnemyRestStage : State
    {
        
        public ShootingEnemyRestStage(Context context) : base(context)
        {
            _currentContext.AiBehaviourController.AiPatrolController.StartPatrolling();
            _currentContext.AiBehaviourController.AiCombatController.StopCombat();
            _currentContext.AiBehaviourController.EffectsFactory.PlayAllEffects(EffectType.EnemyConfuse);
        }

        public override State HandleSpotEnemy(ActorStats actorStats)
        {
            _currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            _currentContext.AiBehaviourController.EffectsFactory.PlayAllEffects(EffectType.Detection);
            
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