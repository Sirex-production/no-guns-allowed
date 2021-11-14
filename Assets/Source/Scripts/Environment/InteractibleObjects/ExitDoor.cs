using Support;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class ExitDoor : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEventController _))
            {
                GameController.Instance.EndLevel(true);
            }
        }
    }
}