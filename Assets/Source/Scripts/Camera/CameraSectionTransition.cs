using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class CameraSectionTransition : MonoBehaviour
    {
        [Required] [SerializeField] private CinemachineVirtualCamera levelOverviewCamera;
        [Required] [SerializeField] private CinemachineVirtualCamera[] levelTransitionCameras;

        private const float GIZMOS_SPHERE_RADIUS = .5f;

        private void Start()
        {
            LevelSectionController.Instance.OnSectionEntered += TransitToCameraWithIndex;
            LevelSectionController.Instance.OnLevelOverviewManaged += OnLevelOverviewManaged;
        }

        private void OnDestroy()
        {
            LevelSectionController.Instance.OnSectionEntered -= TransitToCameraWithIndex;
            LevelSectionController.Instance.OnLevelOverviewManaged -= OnLevelOverviewManaged;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.24f, 0f, 0.55f);

            if(levelOverviewCamera != null)
                Gizmos.DrawSphere(levelOverviewCamera.transform.position, GIZMOS_SPHERE_RADIUS);
            
            if(levelTransitionCameras == null)
                return;
            
            foreach (var levelCamera in levelTransitionCameras)
            {
                if(levelCamera == null)
                    continue;
                
                Gizmos.DrawSphere(levelCamera.transform.position, GIZMOS_SPHERE_RADIUS);
            }
        }

        private void TransitToCameraWithIndex(int cameraSectionIndex)
        {
            if(cameraSectionIndex < 0 || cameraSectionIndex >= levelTransitionCameras.Length || levelTransitionCameras == null || levelTransitionCameras.Length < 1)
                return;

            ResetPrioritiesToZero();
            levelTransitionCameras[cameraSectionIndex].Priority = 10;
        }

        private void OnLevelOverviewManaged(bool isEntered, int currentSection)
        {
            if (isEntered)
                TransitToLevelOverview();
            else
                TransitToCameraWithIndex(currentSection);
        }

        private void TransitToLevelOverview()
        {
            ResetPrioritiesToZero();
            levelOverviewCamera.Priority = 10;
        }

        private void ResetPrioritiesToZero()
        {
            if(levelOverviewCamera != null)
                levelOverviewCamera.Priority = 0;
            
            if(levelTransitionCameras == null)
                return;
            
            foreach (var levelCamera in levelTransitionCameras)
            {
                if(levelCamera == null)
                    continue;

                levelCamera.Priority = 0;
            }
        }
    }
}