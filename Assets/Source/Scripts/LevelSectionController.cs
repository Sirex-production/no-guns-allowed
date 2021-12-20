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
        public event Action<bool, int> OnLevelOverviewManaged;

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
            _sections[sectionNumber].OnPlayerSectionEnter();
            OnSectionEntered?.Invoke(sectionNumber);
        }
        
        public void ExitSection(int sectionNumber)
        {
            CurrentSection = sectionNumber;
            _sections[sectionNumber].OnPlayerSectionExit();
            OnSectionExit?.Invoke(sectionNumber);
        }

        /// <param name="isEntered">Identifies weather level overview was entered or not</param>
        public void ManageLevelOverview(bool isEntered, int currentSection)
        {
            _sections[currentSection].OnLevelOverviewManaged(isEntered);
            OnLevelOverviewManaged?.Invoke(isEntered, currentSection);
        }
    }
}