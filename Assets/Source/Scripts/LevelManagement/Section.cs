using UnityEngine;

namespace Ingame
{
    public class Section : MonoBehaviour
    {
        [SerializeField] private int sectionNumber = -1;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEventController player))
            {
                LevelSectionController.Instance.EnterLevelOverview(sectionNumber);
            }
        }
    }
}