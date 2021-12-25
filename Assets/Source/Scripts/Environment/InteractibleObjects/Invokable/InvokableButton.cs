using Ingame.UI;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class InvokableButton : MonoBehaviour
    {
        [SerializeField] private Invokable[] invokableObjects;
        [Tooltip("Text that will be displayed in log when player approaches the button")]
        [SerializeField] private string logApproachText;
        [Tooltip("Text that will be displayed in log when player interacts with the button")]
        [SerializeField] private string logInteractionText;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _))
                return;
            
            UiController.Instance.ShowInteractableButton(ActivateInvokableObjects);
            UiController.Instance.DisplayLogMessage(logApproachText, LogDisplayType.DisplayAndKeep);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _))
                return;
            
            UiController.Instance.DisplayLogMessage("...", LogDisplayType.DisplayAndKeep);
            UiController.Instance.HideInteractableButton();
        }

        private void ActivateInvokableObjects()
        {
            if(invokableObjects == null || invokableObjects.Length < 1)
                return;
            
            UiController.Instance.DisplayLogMessage(logInteractionText, LogDisplayType.DisplayAndClear);
            
            foreach (var invokableObject in invokableObjects)
            {
                if(invokableObject == null)
                    continue;
                
                invokableObject.Invoke();
            }
        }
    }
}