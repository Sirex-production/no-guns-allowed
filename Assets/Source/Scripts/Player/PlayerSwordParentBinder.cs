using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
    public class PlayerSwordParentBinder : MonoBehaviour
    {
        [SerializeField] private Transform handTransform;
        [SerializeField] private Transform hipTransform;
        [Space(40)]
        [ReadOnly] [SerializeField] private Vector3 inHandLocalPos;
        [ReadOnly] [SerializeField] private Quaternion inHandLocalRotation;
        [Space]
        [ReadOnly] [SerializeField] private Vector3 onHipLocalPos;
        [ReadOnly] [SerializeField] private Quaternion onHipLocalRotation;

        private void Start()
        {
            PlayerEventController.Instance.OnAim += OnAim;
            PlayerEventController.Instance.OnDashPerformed += OnDashPerformed;
            PlayerEventController.Instance.OnFrozenChanged += OnFrozenChanged;
        }

        private void OnDestroy()
        {
            PlayerEventController.Instance.OnAim -= OnAim;
            PlayerEventController.Instance.OnDashPerformed -= OnDashPerformed;
            PlayerEventController.Instance.OnFrozenChanged -= OnFrozenChanged;
        }

        private void OnDashPerformed(Vector3 _)
        {
            TakeSwordToTheHand();
        }

        private void OnAim(Vector3 _)
        {
            HideSwordToTheScabbard();
        }

        private void OnFrozenChanged(bool _)
        {
            HideSwordToTheScabbard();
        }


        private void TakeSwordToTheHand()
        {
            transform.parent = handTransform;
            transform.localPosition = inHandLocalPos;
            transform.localRotation = inHandLocalRotation;
        }
        
        private void HideSwordToTheScabbard()
        {
            transform.parent = hipTransform;
            transform.localPosition = onHipLocalPos;
            transform.localRotation = onHipLocalRotation;
        }

        [Button("AssignPositionInHand")]
        private void AssignPositionInHand()
        {
            inHandLocalRotation = transform.localRotation;
            inHandLocalPos = transform.localPosition;
        }
        
        [Button("AssignPositionOnHip")]
        private void AssignPositionOnHip()
        {
            onHipLocalRotation = transform.localRotation;
            onHipLocalPos = transform.localPosition;
        }
    }
}