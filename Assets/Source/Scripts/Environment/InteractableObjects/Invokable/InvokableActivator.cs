using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class InvokableActivator : MonoBehaviour
    {
        [SerializeField] private List<MonoInvokable> activateOnStartInvokable;
        [SerializeField] private List<MonoInvokable> activateOnDestroyInvokable;

        private void Start()
        {
            foreach (var invokable in activateOnStartInvokable)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }

        private void OnDestroy()
        {
            foreach (var invokable in activateOnDestroyInvokable)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }
    }
}