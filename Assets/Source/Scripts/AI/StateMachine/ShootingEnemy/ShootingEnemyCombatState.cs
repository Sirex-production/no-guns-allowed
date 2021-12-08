namespace Ingame.AI
{
    public class ShootingEnemyCombatState : State
    {

        public ShootingEnemyCombatState(ActorStats actorToAttack, Context context) : base(context)
        {
            _currentContext.AiBehaviourController.AiCombatController.Attack(actorToAttack);
        }
        

        public override State HandleSpotEnemy(ActorStats actorStats)
        {
            return this;
        }

        public override State HandleTakeDamage()
        {
            return this;
        }

        public override State HandleEnterRest()
        {
            //todo enter sleep state
            return new ShootingEnemyRestStage(_currentContext);
        }

        public override State HandleDeath()
        {    
            _currentContext.AiBehaviourController.AiCombatController.StopCombat();
            _currentContext.AiBehaviourController.DestroyActor();

            return this;
        }
    }
}