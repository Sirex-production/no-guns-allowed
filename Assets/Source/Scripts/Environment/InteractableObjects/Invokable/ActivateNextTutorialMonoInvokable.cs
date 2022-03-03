using Ingame.UI;
using Zenject;

namespace Ingame
{
    public class ActivateNextTutorialMonoInvokable : MonoInvokable
    {
        [Inject] private TutorialsManager _tutorialsManager;
        
        public override void Invoke()
        {
            _tutorialsManager.ActivateNext();
            
            base.Invoke();
        }
    }
}