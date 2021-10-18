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

        public override void SpotEnemy()
        {
            //todo play spot animation
            if(Random.Range(0, 100) <= CHANCE_TO_STOP_PATROLLING)
                currentContext.AiBehaviourController.AiPatrolController.StopPatrolling();
            
            currentContext.CurrentState = new ShootingEnemyCombatState();
        }

        public override void TakeDamage()
        {
            //todo die
        }

        public override void EnterRest()
        {
            //todo do nothing
        }
    }
}