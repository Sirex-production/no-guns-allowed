using System;
using Support;
using UnityEngine;

namespace Ingame
{
    public class SectionsManager : MonoSingleton<SectionsManager>
    {
        [SerializeField] private int sectionNumber = -1;

        private int _currentSection = -1;
        
        public event Action<int> OnSectionEnter;
        public event Action OnLevelOverviewEnter;
        public event Action<int> OnLevelOverviewExit;

        public int CurrentSection => _currentSection;

        public void EnterSection(int sectionId)
        {
            _currentSection = sectionId;
            
            OnSectionEnter?.Invoke(sectionId);
        }

        public void EnterLevelOverview()
        {
            OnLevelOverviewEnter?.Invoke();
        }

        public void ExitSectionOverview()
        {
            OnLevelOverviewExit?.Invoke(_currentSection);
        }
    }
}