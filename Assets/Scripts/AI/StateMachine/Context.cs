namespace Ingame.AI
{
    public class Context
    {
        private AiBehaviourController _aiBehaviourController;
        private State _currentState;
        
        public AiBehaviourController AiBehaviourController => _aiBehaviourController;
        
        public State CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                _currentState.currentContext = this;
                _currentState.OnStateEntered();
            }
        }

        public Context(AiBehaviourController aiBehaviourController, State initialState)
        {
            _aiBehaviourController = aiBehaviourController;

            CurrentState = initialState;
        }
        
        public virtual void SpotEnemy()
        {
            CurrentState.HandleSpotEnemy();
        }

        public virtual void TakeDamage()
        {
            CurrentState.HandleTakeDamage();
        }

        public virtual void EnterRest()
        {
            CurrentState.HandleEnterRest();
        }

        public virtual void Die()
        {
            CurrentState.HandleDeath();
        }
    }
}