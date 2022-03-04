using System;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class SectionsManager : MonoBehaviour
    {
        [SerializeField] private int initialStartSection = 0;
        
        [ReadOnly][SerializeField] private int _currentSection = 0;

        [Inject] private InputSystem _inputSystem;
        
        private bool _isInLevelOverview = false;
        
        public event Action<int> OnSectionEnter;
        public event Action OnLevelOverviewEnter;
        public event Action<int> OnLevelOverviewExit;

        public int CurrentSection => _currentSection;
        public bool IsInLevelOverview => _isInLevelOverview;

        protected void Awake()
        {
            _currentSection = initialStartSection;
        }

        public void EnterSection(int sectionId)
        {
            _currentSection = sectionId;
            _inputSystem.Enabled = true;

            OnSectionEnter?.Invoke(sectionId);
        }

        public void EnterLevelOverview()
        {
            _isInLevelOverview = true;
            _inputSystem.Enabled = false;

            OnLevelOverviewEnter?.Invoke();
        }

        public void ExitLevelOverview()
        {
            _isInLevelOverview = false;
            _inputSystem.Enabled = true;
            
            OnLevelOverviewExit?.Invoke(_currentSection);
        }
    }
}