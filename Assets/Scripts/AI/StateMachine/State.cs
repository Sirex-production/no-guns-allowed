namespace Ingame.AI
{
    public abstract class State
    {
        public Context currentContext;

        public abstract void OnStateEntered();
        public abstract void HandleSpotEnemy();
        public abstract void HandleTakeDamage();
        public abstract void HandleEnterRest();
        public abstract void HandleDeath();
    }
}