using Ingame.UI;
using UnityEngine;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class ActivateNextTutorialTrigger : MonoBehaviour
    {
        [Inject]
        private TutorialsManager _tutorialsManager;
        
        private void OnTriggerEnter(Collider other)
        {
            _tutorialsManager.ActivateNext();
            Destroy(this);
        }
    }
}