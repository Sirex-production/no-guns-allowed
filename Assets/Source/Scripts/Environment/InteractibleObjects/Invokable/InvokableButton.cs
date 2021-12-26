using Extensions;
using Ingame.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class InvokableButton : MonoBehaviour
    {
        [SerializeField] private Invokable[] invokableObjects;
        [SerializeField] private bool deactivatePanelAfterInteraction = false;
        [Tooltip("Text that will be displayed in log when player approaches the button")]
        [SerializeField] private string logApproachText;
        [Tooltip("Text that will be displayed in log when player interacts with the button")]
        [SerializeField] private string logInteractionText;

        private bool _isWorking = true;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _) || !_isWorking)
                return;
            
            UiController.Instance.ShowInteractableButton(ActivateInvokableObjects);
            UiController.Instance.DisplayLogMessage(logApproachText, LogDisplayType.DisplayAndKeep);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _) || !_isWorking)
                return;
            
            UiController.Instance.DisplayLogMessage("...", LogDisplayType.DisplayAndKeep);
            UiController.Instance.HideInteractableButton();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, "Invokable button", GUI.skin.button);
            
            if(invokableObjects == null || invokableObjects.Length < 1)
                return;
            
            foreach (var invokableObject in invokableObjects)
            {
                if(invokableObject == null)
                    continue;
                
                Handles.color = Color.gray;
                Handles.DrawAAPolyLine(transform.position, invokableObject.transform.position);
            }
        }
#endif
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

            if (deactivatePanelAfterInteraction)
                _isWorking = false;
        }
    }
}