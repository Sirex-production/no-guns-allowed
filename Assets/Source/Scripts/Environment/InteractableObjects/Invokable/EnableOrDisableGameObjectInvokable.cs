using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class EnableOrDisableGameObjectInvokable : MonoInvokable
    {
        [SerializeField] private bool enableGameObject;
        [Required]
        [SerializeField] private GameObject gameObjectToProcess;
        
        public override void Invoke()
        {
            gameObjectToProcess.SetActive(enableGameObject);
            
            base.Invoke();
        }
    }
}