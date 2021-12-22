using System;
using Support;

namespace Ingame
{
    public class LevelSectionController : MonoSingleton<LevelSectionController>
    {
        public event Action<int> OnSectionEnter;
        public event Action<int> OnSectionExit;
        public event Action OnLevelOverviewEnter;
        public event Action OnLevelOverviewExit;

        public int CurrentSection { get; private set; }
        
        public void EnterSection(int sectionNumber)
        {
            CurrentSection = sectionNumber;
            OnSectionEnter?.Invoke(sectionNumber);
        }
        
        public void ExitSection(int sectionNumber)
        {
            CurrentSection = sectionNumber;
            OnSectionExit?.Invoke(sectionNumber);
        }

        public void EnterLevelOverview(int enterSectionId)
        {
            OnLevelOverviewEnter?.Invoke();
        }

        public void ExitLevelOverview()
        {
            OnLevelOverviewExit?.Invoke();
        }

    }
}