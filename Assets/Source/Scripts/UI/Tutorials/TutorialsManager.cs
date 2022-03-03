using System.Collections.Generic;
using Extensions;
using Support;
using UnityEngine;

namespace Ingame.UI
{
    public class TutorialsManager : MonoBehaviour
    {
        [SerializeField] private List<MonoTutorial> tutorials;

        private int _currentTutorialIndex = 0;

        private void Start()
        {
            TurnOffTutorials();
        }

        private void TurnOffTutorials()
        {
            if(tutorials == null)
                return;

            foreach (var tutorial in tutorials)
            {
                if(tutorial == null)
                    continue;
                
                tutorial.SetGameObjectInactive();
            }
        }

        public void ActivateNext()
        {
            if(tutorials == null || _currentTutorialIndex >= tutorials.Count)
                return;
            
            var tutorial = tutorials[_currentTutorialIndex];
            
            if(tutorial == null)
                return;
            
            tutorial.SetGameObjectActive();
            tutorial.Activate();
            
            _currentTutorialIndex++;
        }
    }
}