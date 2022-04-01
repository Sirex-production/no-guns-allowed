using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class ActivateInvokablesTrigger : MonoBehaviour
    {
        [SerializeField] private bool destroyComponentAfterInvoke = false;
        [SerializeField] private List<MonoInvokable> activateOnTriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            foreach (var invokable in activateOnTriggerEnter)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
            
            if(destroyComponentAfterInvoke)
                Destroy(this);
        }
    }
}