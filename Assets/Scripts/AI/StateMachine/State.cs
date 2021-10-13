namespace Ingame.AI
{
    public abstract class State
    {
        protected Context _currentContext;

        public State(Context currentContext)
        {
            _currentContext = currentContext;
            OnStateEntered();
        }

        protected abstract void OnStateEntered();
        
        public abstract void SpotEnemy();
        public abstract void TakeDamage();
        public abstract void EnterRest();
    }
}