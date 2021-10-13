namespace Ingame.AI
{
    public abstract class State
    {
        public Context currentContext;

        public abstract void OnStateEntered();
        public abstract void SpotEnemy();
        public abstract void TakeDamage();
        public abstract void EnterRest();
    }
}