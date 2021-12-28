using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ingame
{
    public abstract class SectionPart : MonoBehaviour
    {
        [SerializeField] protected int boundedSectionId = -1;
        
        protected virtual void Start()
        {
            SectionsManager.Instance.OnSectionEnter += OnSectionEnter;
            SectionsManager.Instance.OnLevelOverviewEnter += OnLevelOverviewEnter;
            SectionsManager.Instance.OnLevelOverviewExit += OnLevelOverviewExit;
        }

        protected virtual void OnDestroy()
        {
            SectionsManager.Instance.OnSectionEnter -= OnSectionEnter;
            SectionsManager.Instance.OnLevelOverviewEnter -= OnLevelOverviewEnter;
            SectionsManager.Instance.OnLevelOverviewExit -= OnLevelOverviewExit;
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