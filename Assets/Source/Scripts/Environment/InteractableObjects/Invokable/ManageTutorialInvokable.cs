using Ingame.UI;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class ManageTutorialInvokable : MonoInvokable
    {
        [SerializeField] private bool activateNext = true;
        [SerializeField] private bool completeCurrent = false;
        
        [Inject] private TutorialsManager _tutorialsManager;
        
        public override void Invoke()
        {
            if(completeCurrent)
                _tutorialsManager.CompleteCurrentTutorial();
            
            if(activateNext)
                _tutorialsManager.ActivateNext();
            
            base.Invoke();
        }
    }
}