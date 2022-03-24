using System;
using System.Linq;
using Cinemachine;
using Extensions;
using Ingame.Graphics;
using NaughtyAttributes;
using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public class CameraTransiter : MonoBehaviour
    {
        [Required]
        [SerializeField] private CinemachineVirtualCamera playerFocusCamera;
        [Required]
        [SerializeField] private CinemachineVirtualCamera levelOverviewCamera;
        [SerializeField] private CinemachineVirtualCamera cutSceneCamera;
        [SerializeField] private CinemachineVirtualCamera[] levelTransitionCameras;

        private const float GIZMOS_SPHERE_RADIUS = .5f;

        [Inject] private GameController _gameController;
        [Inject] private SectionsManager _sectionsManager;
        [Inject] private EffectsManager _effectsManager;

        public event Action<CinemachineVirtualCamera> OnVirtualCameraChanged;

        private void Start()
        {
            _gameController.OnGameplayStarted += OnGameplayStarted;
            _sectionsManager.OnSectionEnter += TransitToCameraWithIndex;
            _sectionsManager.OnLevelOverviewEnter += OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit += OnLevelOverviewExit;
            _effectsManager.OnPlayerDeathEffectPlayed += FocusOnPlayer;
        }

        private void OnDestroy()
        {
            _gameController.OnGameplayStarted -= OnGameplayStarted;
            _sectionsManager.OnSectionEnter -= TransitToCameraWithIndex;
            _sectionsManager.OnLevelOverviewEnter -= OnLevelOverviewEnter;
            _sectionsManager.OnLevelOverviewExit -= OnLevelOverviewExit;
            _effectsManager.OnPlayerDeathEffectPlayed -= FocusOnPlayer;
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

        private void OnGameplayStarted()
        {
            ChangeVirtualCamera(levelTransitionCameras.First(cam => cam != null));
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

        private void FocusOnPlayer()
        {
            ChangeVirtualCamera(playerFocusCamera);
        }

        private void ResetPrioritiesToZero()
        {
            levelOverviewCamera.Priority = 0;
            playerFocusCamera.Priority = 0;
            if(cutSceneCamera != null)
                cutSceneCamera.Priority = 0;
            
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