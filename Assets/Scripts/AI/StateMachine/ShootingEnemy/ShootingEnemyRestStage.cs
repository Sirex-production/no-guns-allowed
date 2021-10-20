using Ingame.Graphics;
using Random = UnityEngine.Random;

namespace Ingame.AI
{
    public class ShootingEnemyRestStage : State
    {
        private const float CHANCE_TO_STOP_PATROLLING= 40;
        
        public override void OnStateEntered()
        {
            currentContext.AiBehaviourController.AiPatrolController.StartPatrolling();
        }

        public override void HandleSpotEnemy()
        {
            //todo play spot animation
            if(Random.Range(0, 100) <= CHANCE_TO_STOP_PATROLLING)
                currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            
            currentContext.CurrentState = new ShootingEnemyCombatState();
        }

        public override void HandleTakeDamage()
        {
            //todo die
        }

        public override void HandleEnterRest()
        {
            //todo do nothing
        }

        public override void HandleDeath()
        {
            currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            if(currentContext.AiBehaviourController.EffectsManager != null)
                currentContext.AiBehaviourController.EffectsManager.PlayAllEffects(EffectType.Destruction);
            currentContext.AiBehaviourController.DestroyActor();
        }
    }
}