namespace Ingame.AI
{
    public class ShootingEnemyCombatState : State
    {
        public ShootingEnemyCombatState(Context currentContext) : base(currentContext) { }

        protected override void OnStateEntered()
        {
            //todo shoot player
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
            _currentContext.CurrentState = new ShootingEnemyRestStage(_currentContext);
        }
    }
}