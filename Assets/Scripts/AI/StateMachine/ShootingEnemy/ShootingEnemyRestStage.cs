namespace Ingame.AI
{
    public class ShootingEnemyRestStage : State
    {
        public override void OnStateEntered()
        {
            currentContext.AiBehaviourController.AiPatrolController.StartPatrolling();
        }

        public override void SpotEnemy()
        {
            //todo play spot animation
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