namespace Ingame.AI
{
    public abstract class Context
    {
        private AiBehaviourController _aiBehaviourController;

        public AiBehaviourController AiBehaviourController => _aiBehaviourController;
        public State CurrentState { get; set; }

        public Context(AiBehaviourController aiBehaviourController)
        {
            _aiBehaviourController = aiBehaviourController;

            CurrentState = CreateInitialState();
        }

        protected abstract State CreateInitialState();
        
        public virtual void SpotEnemy()
        {
            CurrentState.SpotEnemy();
        }

        public virtual void TakeDamage()
        {
            CurrentState.TakeDamage();
        }

        public virtual void EnterRest()
        {
            CurrentState.EnterRest();
        }
    }
}