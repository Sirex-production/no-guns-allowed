using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class Section : MonoBehaviour
    {
        [SerializeField] private int sectionId;

        private List<ISectionPart> _sectionParts = new List<ISectionPart>();
        
        public int SectionId => sectionId;
        private void Start()
        {
            LevelSectionController.Instance.AddSection(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ISectionPart sectionPart))
            {
                _sectionParts.Add(sectionPart);
                sectionPart.OnSectionAdded();
            }

            if (other.TryGetComponent(out PlayerEventController player))
                this.DoAfterNextFrameCoroutine(NotifyThatPlayerEnteredTheSection);
        }

        private void NotifyThatPlayerEnteredTheSection()
        {
            LevelSectionController.Instance.EnterSection(sectionId);
        }

        public void OnPlayerSectionEnter()
        {
            foreach (var section in _sectionParts) 
                section.OnPlayerSectionEnter();
        }
        
        public void OnPlayerSectionExit()
        {
            foreach (var section in _sectionParts) 
                section.OnPlayerSectionExit();
        }

        public void OnLevelOverviewManaged(bool isEntered)
        {
            foreach (var section in _sectionParts)
            {
                section.OnLevelOverviewManaged(isEntered);
            }
        }
    }
}