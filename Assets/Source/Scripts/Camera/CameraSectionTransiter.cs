using System;
using System.Linq;
using Cinemachine;
using Extensions;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class CameraSectionTransiter : MonoBehaviour
    {
        [Required]
        [SerializeField] private CinemachineVirtualCamera levelOverviewCamera;
        [SerializeField] private CinemachineVirtualCamera[] levelTransitionCameras;

        private const float GIZMOS_SPHERE_RADIUS = .5f;

        [Inject]
        private SectionsManager _sectionsManager;

        public event Action<CinemachineVirtualCamera> OnVirtualCameraChanged;

        private void Start()
        {
            _sectionsManager.OnSectionEnter += TransitToCameraWithIndex;
            _sectionsManager.OnLevelOverviewEnter += OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit += OnLevelOverviewExit;

            this.DoAfterNextFrameCoroutine(() => ChangeVirtualCamera(levelTransitionCameras.First(cam => cam != null)));
        }

        private void OnDestroy()
        {
            _sectionsManager.OnSectionEnter -= TransitToCameraWithIndex;
            _sectionsManager.OnLevelOverviewEnter -= OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit -= OnLevelOverviewExit;
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


            ChangeVirtualCamera(levelTransitionCameras[cameraSectionIndex]);
        }

        private void OnLevelOverviewEnter()
        {
            ChangeVirtualCamera(levelOverviewCamera);
        }

        private void OnLevelOverviewExit(int currentSectionId)
        {
            TransitToCameraWithIndex(currentSectionId);
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

        private void ChangeVirtualCamera(CinemachineVirtualCamera actualVirtualCamera)
        {
            ResetPrioritiesToZero();
            actualVirtualCamera.Priority = 10;
            OnVirtualCameraChanged?.Invoke(actualVirtualCamera);
        }
    }
}