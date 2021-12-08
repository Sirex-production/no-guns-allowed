namespace Ingame.AI
{
    public abstract class State
    {
        protected Context _currentContext;

        public State(Context context)
        {
            _currentContext = context;
        }

        public abstract State HandleSpotEnemy(ActorStats actorStats);
        public abstract State HandleTakeDamage();
        public abstract State HandleEnterRest();
        public abstract State HandleDeath();
    }
}