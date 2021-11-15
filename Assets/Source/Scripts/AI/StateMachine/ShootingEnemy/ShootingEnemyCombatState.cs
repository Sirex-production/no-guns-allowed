namespace Ingame.AI
{
    public class ShootingEnemyCombatState : State
    {
        public override void OnStateEntered()
        {
            currentContext.AiBehaviourController.AiCombatController.Attack(PlayerEventController.Instance.StatsController);
        }

        public override void HandleSpotEnemy()
        {
            //todo do nothing
        }

        public override void HandleTakeDamage()
        {
            //todo play effect
        }

        public override void HandleEnterRest()
        {
            //todo enter sleep state
            currentContext.CurrentState = new ShootingEnemyRestStage();
        }

        public override void HandleDeath()
        {
            currentContext.AiBehaviourController.AiCombatController.StopCombat();
            currentContext.AiBehaviourController.DestroyActor();
        }
    }
}