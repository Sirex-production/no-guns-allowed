namespace Ingame.AI
{
    public class ShootingEnemyContext : Context
    {
        public ShootingEnemyContext(AiBehaviourController aiBehaviourController) : base(aiBehaviourController) { }
        
        protected override State CreateInitialState()
        {
            return new ShootingEnemyRestStage(this);
        }
    }
}