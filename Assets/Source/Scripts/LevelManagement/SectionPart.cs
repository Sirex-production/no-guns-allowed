using Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public abstract class SectionPart : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private int boundedSectionId = -1;
        
        public virtual void OnSectionAdded(int sectionId)
        {
            boundedSectionId = sectionId;
            this.SafeDebug(sectionId);
        }
        
        public abstract void OnPlayerSectionEnter();
        public abstract void OnPlayerSectionExit();
        public abstract void OnLevelOverviewEnter();
        public abstract void OnLevelOverviewExit();
    }
}