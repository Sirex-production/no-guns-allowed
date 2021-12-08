using System;

namespace Ingame.AI
{
    public class Context
    {
        private AiBehaviourController _aiBehaviourController;
        private State _currentState;
        
        public AiBehaviourController AiBehaviourController => _aiBehaviourController;
        public State CurrentState
        {
            set => _currentState = value;
        }

        public Context(AiBehaviourController aiBehaviourController)
        {
            _aiBehaviourController = aiBehaviourController;
        }
        
        public virtual void SpotEnemy(ActorStats actorStats)
        {
            _currentState = _currentState.HandleSpotEnemy(actorStats);
        }

        public virtual void TakeDamage()
        {
            _currentState = _currentState.HandleTakeDamage();
        }

        public virtual void EnterRest()
        {
            _currentState = _currentState.HandleEnterRest();
        }

        public virtual void Die()
        {
            _currentState = _currentState.HandleDeath();
        }
    }
}