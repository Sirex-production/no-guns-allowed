using System;
using System.Collections;
using Extensions;
using Ingame;
using Ingame.AI;
using Support;
using UnityEngine;

public class RechargingStation : MonoBehaviour
{
    private enum State
    {
        OnCooldown,
        Active,
        Inactive
    }

    [SerializeField] private int regenerationSpeedMultiplier;
    [SerializeField] private float cooldown;

    [Header("State Meshes")] 
    [SerializeField] private MeshRenderer activeMesh;
    [SerializeField] private MeshRenderer inactiveMesh;
    [SerializeField] private MeshRenderer cooldownMesh;

    private State _state;

    private void Awake()
    {
        _state = State.Inactive;
        inactiveMesh.SetGameObjectActive();

        activeMesh.SetGameObjectInactive();
        cooldownMesh.SetGameObjectInactive();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != PlayerEventController.Instance.StatsController ||
            _state != State.Inactive)
            return;

        SwitchState(State.Active);
        PlayerEventController.Instance.StatsController.ChargeRegenerationTimeModifier = regenerationSpeedMultiplier;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != PlayerEventController.Instance.StatsController ||
            _state != State.Active) 
            return;

        SwitchState(State.OnCooldown);
        StartCoroutine(CooldownRoutine());
        PlayerEventController.Instance.StatsController.ChargeRegenerationTimeModifier = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out HitBox hitbox) ||
            hitbox.AttachedActorStats != PlayerEventController.Instance.StatsController ||
            _state != State.Inactive) 
            return;

        SwitchState(State.Active);
        PlayerEventController.Instance.StatsController.ChargeRegenerationTimeModifier = regenerationSpeedMultiplier;
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
