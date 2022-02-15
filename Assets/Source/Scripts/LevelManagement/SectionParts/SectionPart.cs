using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame
{
    public abstract class SectionPart : MonoBehaviour
    {
        [SerializeField] protected int boundedSectionId = -1;

        [Inject] 
        private SectionsManager _sectionsManager;
        
        protected virtual void Start()
        {
            _sectionsManager.OnSectionEnter += OnSectionEnter;
            _sectionsManager.OnLevelOverviewEnter += OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit += OnLevelOverviewExit;
        }

        protected virtual void OnDestroy()
        {
            _sectionsManager.OnSectionEnter -= OnSectionEnter;
            _sectionsManager.OnLevelOverviewEnter -= OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit -= OnLevelOverviewExit;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var style = GUI.skin.box;
            Handles.Label(transform.position, $"section#{boundedSectionId}", style);
        }
#endif

        protected abstract void OnSectionEnter(int sectionId);
        protected abstract void OnLevelOverviewEnter();
        protected abstract void OnLevelOverviewExit(int currentSectionId);
    }
}