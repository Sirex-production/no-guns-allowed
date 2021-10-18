using System.Collections.Generic;
using Extensions;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class TutorialsManager : MonoSingleton<TutorialsManager>
    {
        [SerializeField] private List<MonoTutorial> tutorials;

        private int _currentTutorialIndex = 0;
        
        private void Start()
        {
            ActivateNext();
        }

        public void ActivateNext()
        {
            if(tutorials == null || _currentTutorialIndex >= tutorials.Count)
                return;

            var tutorial = tutorials[_currentTutorialIndex]; 
            tutorial.SetGameObjectActive();
            tutorial.Activate();
            
            _currentTutorialIndex++;
        }
    }
}