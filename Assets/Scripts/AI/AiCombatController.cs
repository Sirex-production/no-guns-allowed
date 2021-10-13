using UnityEngine;

namespace Ingame.AI
{
    [RequireComponent(typeof(AiBehaviourController))]
    public class AiCombatController : MonoBehaviour
    {
        private AiBehaviourController _aiBehaviourController;

        private void Awake()
        {
            _aiBehaviourController = GetComponent<AiBehaviourController>();
        }
    }
}