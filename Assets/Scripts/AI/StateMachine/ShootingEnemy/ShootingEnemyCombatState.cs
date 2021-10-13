namespace Ingame.AI
{
    public class ShootingEnemyCombatState : State
    {
        public override void OnStateEntered()
        {
            currentContext.AiBehaviourController.AiCombatController.Attack(PlayerEventController.Instance.StatsController);
        }

        public override void SpotEnemy()
        {
            //todo do nothing
        }

        public override void TakeDamage()
        {
            //todo die
        }

        public override void EnterRest()
        {
            //todo enter sleep state
            currentContext.CurrentState = new ShootingEnemyRestStage();
        }
    }
}