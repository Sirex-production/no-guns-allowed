using System.Collections;
using Extensions;
using Ingame;
using Ingame.AI;
using Ingame.UI;
using UnityEngine;
using Zenject;

public class RechargingStation : MonoBehaviour
{
    private enum State
    {
        OnCooldown,
        Active,
        Inactive
    }
    
    [Header("Station options")]
    [SerializeField] private int regenerationSpeedMultiplier;
    [SerializeField] private float cooldown;
    [Header("Ui properties")] 
    [SerializeField] private string logMessageWhenEntered;
    [Header("State Meshes")] 
    [SerializeField] private MeshRenderer activeMesh;
    [SerializeField] private MeshRenderer inactiveMesh;
    [SerializeField] private MeshRenderer cooldownMesh;
    
    [Inject] private UiController _uiController;

    private PlayerEventController _playerEventController;
    private State _state;

    private void Awake()
    {
        _state = State.Inactive;
        inactiveMesh.SetGameObjectActive();

        activeMesh.SetGameObjectInactive();
        cooldownMesh.SetGameObjectInactive();
    }

    private void Start()
    {
        _playerEventController = PlayerEventController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != _playerEventController.StatsController ||
            _state != State.Inactive)
            return;

        SwitchState(State.Active);
        _playerEventController.StatsController.ChargeRegenerationTimeModifier = regenerationSpeedMultiplier;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != _playerEventController.StatsController ||
            _state != State.Active) 
            return;

        SwitchState(State.OnCooldown);
        StartCoroutine(CooldownRoutine());
        _playerEventController.StatsController.ChargeRegenerationTimeModifier = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != _playerEventController.StatsController ||
            _state != State.Inactive) 
            return;

        SwitchState(State.Active);
        _playerEventController.StatsController.ChargeRegenerationTimeModifier = regenerationSpeedMultiplier;
        
    }
    private IEnumerator CooldownRoutine()
    {
        if(_state != State.OnCooldown)
            this.SafeDebug($"A logical error in the FSM: the state should not be {_state} when entering cooldown", LogType.Error);

        yield return new WaitForSeconds(cooldown);
        SwitchState(State.Inactive);
    }

    private void SwitchState(State state)
    {
        switch (_state)
        {
            case State.Active:
                activeMesh.SetGameObjectInactive();
                break;
            
            case State.Inactive:
                inactiveMesh.SetGameObjectInactive();
                break;
            
            case State.OnCooldown:
                cooldownMesh.SetGameObjectInactive();
                break;
        }

        _state = state;

        switch (_state)
        {
            case State.Active:
                activeMesh.SetGameObjectActive();
                _uiController.DisplayLogMessage(logMessageWhenEntered, LogDisplayType.DisplayAndClear);
                break;
            
            case State.Inactive:
                inactiveMesh.SetGameObjectActive();
                break;
            
            case State.OnCooldown:
                cooldownMesh.SetGameObjectActive();
                break;
        }
    }
}
