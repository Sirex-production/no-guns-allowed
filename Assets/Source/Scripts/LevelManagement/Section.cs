using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class Section : MonoBehaviour
    {
        [SerializeField] private int sectionId;

        private List<SectionPart> _sectionParts = new List<SectionPart>();
        
        public int SectionId => sectionId;

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void Start()
        {
            LevelSectionController.Instance.AddSection(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SectionPart sectionPart))
            {
                _sectionParts.Add(sectionPart);
                sectionPart.OnSectionAdded(sectionId);
            }

            if (other.TryGetComponent(out PlayerEventController player))
                this.DoAfterNextFrameCoroutine(NotifyThatPlayerEnteredTheSection);
        }

        private void NotifyThatPlayerEnteredTheSection()
        {
            LevelSectionController.Instance.EnterSection(sectionId);
        }

        public void OnSectionEnter()
        {
            foreach (var section in _sectionParts) 
                section.OnPlayerSectionEnter();
        }
        
        public void OnSectionExit()
        {
            foreach (var section in _sectionParts) 
                section.OnPlayerSectionExit();
        }

        public void OnLevelOverviewEnter()
        {
            foreach (var section in _sectionParts)
            {
                section.OnLevelOverviewEnter();
            }
        }
        
        public void OnLevelOverviewExit()
        {
            foreach (var section in _sectionParts)
            {
                section.OnLevelOverviewExit();
            }
        }
    }
}