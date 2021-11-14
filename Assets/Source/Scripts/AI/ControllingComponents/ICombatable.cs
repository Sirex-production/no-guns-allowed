namespace Ingame.AI
{
    public interface ICombatable
    {
        public void Attack(ActorStats actorStats);
        public void StopCombat();
    }
}