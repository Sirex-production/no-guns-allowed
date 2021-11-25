using Cinemachine;
using NaughtyAttributes;
using Support;
using UnityEngine;

namespace Ingame
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [Required] [SerializeField] private CinemachineVirtualCamera levelOverviewCamera;
        [Required] [SerializeField] private CinemachineVirtualCamera[] levelTransitionCameras;

        private const float GIZMOS_SPHERE_RADIUS = .5f;

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

        public void TransitToLevelOverview()
        {
            ResetPrioritiesToZero();
            levelOverviewCamera.Priority = 10;
        }

        public void TransitToCameraWithIndex(int index)
        {
            if(index < 0 || index >= levelTransitionCameras.Length || levelTransitionCameras == null || levelTransitionCameras.Length < 1)
                return;

            ResetPrioritiesToZero();
            levelTransitionCameras[index].Priority = 10;
        }
    }
}