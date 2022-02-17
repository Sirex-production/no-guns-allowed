using Support;
using UnityEngine;
using Zenject;

namespace Ingame
{
    [RequireComponent(typeof(Collider))]
    public class ExitDoor : MonoBehaviour
    {
        [Inject] private GameController _gameController;
        
        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEventController player))
            {
                if(player.TryGetComponent(out Rigidbody playerRigidBody))
                    playerRigidBody.velocity = Vector3.zero;

                _gameController.EndLevel(true);
            }
        }
    }
}