namespace Ingame.AI
{
    public class ShootingEnemyRestStage : State
    {
        public ShootingEnemyRestStage(Context currentContext) : base(currentContext) { }

        protected override void OnStateEntered()
        {
            _currentContext.AiBehaviourController.AiPatrolController.StartPatrolling();
        }

        public override void SpotEnemy()
        {
            //todo play spot animation
            _currentContext.CurrentState = new ShootingEnemyCombatState(_currentContext);
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