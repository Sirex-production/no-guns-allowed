using System;
using System.Collections.Generic;
using Support;

namespace Ingame
{
    public class LevelSectionController : MonoSingleton<LevelSectionController>
    {
        private Dictionary<int, Section> _sections = new Dictionary<int, Section>(12);
        
        public event Action<int> OnSectionEntered;
        public event Action<int> OnSectionExit;
        public event Action<int> OnLevelOverviewEnter;
        public event Action<int> OnLevelOverviewExit;

        public int CurrentSection { get; private set; }

        public void AddSection(Section section)
        {
            if (_sections.ContainsKey(section.SectionId))
                throw new ArgumentException($"There is more then one sections with if {section.SectionId}");
            
            _sections.Add(section.SectionId, section);
        }

        public void EnterSection(int sectionNumber)
        {
            CurrentSection = sectionNumber;
            _sections[sectionNumber].OnSectionEnter();
            OnSectionEntered?.Invoke(sectionNumber);
        }
        
        public void ExitSection(int sectionNumber)
        {
            CurrentSection = sectionNumber;
            _sections[sectionNumber].OnSectionExit();
            OnSectionExit?.Invoke(sectionNumber);
        }

        /// <param name="isEntered">Identifies weather level overview was entered or not</param>
        public void EnterLevelOverview(int enterSectionId)
        {
            _sections[enterSectionId].OnLevelOverviewEnter();
            OnLevelOverviewEnter?.Invoke(enterSectionId);
        }

        public void ExitLevelOverview(int exitSectionId)
        {
            _sections[exitSectionId].OnLevelOverviewExit();
            OnLevelOverviewExit?.Invoke(exitSectionId);
        }

    }
}