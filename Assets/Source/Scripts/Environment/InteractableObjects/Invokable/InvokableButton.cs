using Ingame.UI;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame
{
    //TODO: review the possibility of renaming class
    //it's confusing that both monoBehaviour and invokable classes have the same "Invokable" prefix
    [RequireComponent(typeof(Collider))]
    public class InvokableButton : MonoBehaviour
    {
        [Tooltip("Text that will be displayed in log when player approaches the button")]
        [SerializeField] private string logApproachText;
        [Tooltip("Text that will be displayed in log when player interacts with the button")]
        [SerializeField] private string logInteractionText;
        [Space]
        [SerializeField] private bool deactivatePanelAfterInteraction = false;
        [ShowIf(nameof(deactivatePanelAfterInteraction))]
        [SerializeField] private MonoInvokable[] invokableObjectWhenDisabledInvoked;
        [SerializeField] private MonoInvokable[] invokableObjects;

        [Inject] private UiController _uiController;

        private bool _isWorking = true;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _) || !_isWorking)
                return;
            
            _uiController.ShowInteractableButton(ActivateInvokableObjects);
            _uiController.DisplayLogMessage(logApproachText, LogDisplayType.DisplayAndKeep);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent(out PlayerEventController _) || !_isWorking)
                return;
            
            _uiController.DisplayLogMessage("...", LogDisplayType.DisplayAndKeep);
            _uiController.HideInteractableButton();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, "Invokable button", GUI.skin.button);
            
            if(invokableObjects == null || invokableObjects.Length < 1)
                return;
            
            var currentObjectPos = transform.position;
            
            foreach (var invokableObject in invokableObjects)
            {
                if(invokableObject == null)
                    continue;

                var invokableObjectPos = invokableObject.transform.position;
                var halfHeight = currentObjectPos.y - invokableObjectPos.y;
                var tangentOffset = Vector3.up * halfHeight;

                Handles.DrawBezier(currentObjectPos, 
                    invokableObjectPos,
                    currentObjectPos - tangentOffset,
                    invokableObjectPos + tangentOffset,
                    Color.gray,
                    EditorGUIUtility.whiteTexture, 1);
            }
        }
#endif
        private void ActivateInvokableObjects()
        {
            if(invokableObjects == null || invokableObjects.Length < 1)
                return;

            _uiController.DisplayLogMessage(logInteractionText, LogDisplayType.DisplayAndClear);
            
            foreach (var invokableObject in invokableObjects)
            {
                if(invokableObject == null)
                    continue;
                
                invokableObject.Invoke();
            }

            if (deactivatePanelAfterInteraction)
                Disable();
        }

        private void Disable()
        {
            foreach (var invokableObject in invokableObjectWhenDisabledInvoked)
            {
                if(invokableObject == null)
                    continue;
                
                invokableObject.Invoke();
            }
            
            _isWorking = false;
        }
    }
}