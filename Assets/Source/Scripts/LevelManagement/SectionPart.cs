using UnityEngine;

namespace Ingame
{
    public abstract class SectionPart : MonoBehaviour
    {
        // [ReadOnly] [SerializeField] private int boundedSectionId = -1;

        protected virtual void Start()
        {
            LevelSectionController.Instance.OnSectionEnter += OnSectionEnter;
            LevelSectionController.Instance.OnSectionExit += OnSectionExit;
            LevelSectionController.Instance.OnLevelOverviewEnter += OnLevelOverviewEnter;
            LevelSectionController.Instance.OnLevelOverviewExit += OnLevelOverviewExit;
        }

        protected abstract void OnSectionEnter(int sectionId);
        protected abstract void OnSectionExit(int sectionId);
        protected abstract void OnLevelOverviewEnter();
        protected abstract void OnLevelOverviewExit();
    }
}