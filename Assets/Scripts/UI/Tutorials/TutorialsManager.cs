using System.Collections.Generic;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class TutorialsManager : MonoSingleton<TutorialsManager>
    {
        [SerializeField] private List<Tutorial> tutorials;

        private int _currentTutorialIndex = 0;
        
        private void Awake()
        {
            ActivateNext();
        }

        public void ActivateNext()
        {
            if(tutorials == null || _currentTutorialIndex >= tutorials.Count)
                return;
            
            tutorials[_currentTutorialIndex].Activate();
            _currentTutorialIndex++;
        }
    }
}