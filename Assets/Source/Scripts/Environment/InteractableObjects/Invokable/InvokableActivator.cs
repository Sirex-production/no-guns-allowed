using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class InvokableActivator : MonoInvokable
    {
        [SerializeField] private List<MonoInvokable> activateOnStart;
        [SerializeField] private List<MonoInvokable> activateOnInvoke;
        [SerializeField] private List<MonoInvokable> activateOnDestroy;

        private void Start()
        {
            foreach (var invokable in activateOnStart)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }

        private void OnDestroy()
        {
            foreach (var invokable in activateOnDestroy)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }

        public override void Invoke()
        {
            foreach (var invokable in activateOnInvoke)
            {
                if(invokable == null)
                    continue;
                
                invokable.Invoke();
            }
        }
    }
}