using System.Collections;
using System.Collections.Generic;
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

    [Header("Visual options")] 
    [SerializeField] private float transitionTime;
    [SerializeField] [ColorUsage(false, true)] private Color activeColor;
    [SerializeField] [ColorUsage(false, true)] private Color inactiveColor;
    [SerializeField] [ColorUsage(false, true)] private Color cooldownColor;

    [Header("References")] [SerializeField]
    private List<ParticleSystem> particleSystems;

    [Inject] private UiController _uiController;

    private static readonly int EMISSION_COLOR = Shader.PropertyToID("EmissionColor");

    private PlayerEventController _playerEventController;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private State _state;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
        _state = State.Inactive;
    }

    private void Start()
    {
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(EMISSION_COLOR, inactiveColor);
        _renderer.SetPropertyBlock(_propertyBlock);

        _playerEventController = PlayerEventController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckSwitchConditions(other, State.Inactive))
            return;

        SwitchState(State.Active);
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckSwitchConditions(other, State.Active)) 
            return;

        SwitchState(State.OnCooldown);
    }

    private void OnTriggerStay(Collider other)
    {
        if (CheckSwitchConditions(other, State.Inactive)) 
            return;

        SwitchState(State.Active);
    }

    private IEnumerator ColorShiftRoutine(Color currentColor, Color newColor)
    {
        var timeLeft = transitionTime;

        while (timeLeft > 0)
        {
            var timeClamped = 1 - timeLeft / transitionTime;
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(EMISSION_COLOR, Color.Lerp(currentColor, newColor, timeClamped));
            _renderer.SetPropertyBlock(_propertyBlock);

            timeLeft -= Time.deltaTime * Time.timeScale;
            yield return null;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        if(_state != State.OnCooldown)
            this.SafeDebug($"A logical error in the FSM: the state should not be {_state} when entering cooldown", LogType.Error);

        yield return new WaitForSeconds(cooldown);
        SwitchState(State.Inactive);
    }

    private bool CheckSwitchConditions(Collider other, State expectedState)
    {
        var otherIsNotThePlayer = other is null || 
                               !other.TryGetComponent(out HitBox hitbox) ||
                               hitbox.AttachedActorStats != _playerEventController.StatsController;
        
        return otherIsNotThePlayer || _state != expectedState;
    }


    /// <param name="state">true when launching, false when stopping</param>
    private void ToggleParticleSystems(bool state)
    {
        if (state)
            foreach (var system in particleSystems)
                system.Play();

        else
            foreach (var system in particleSystems)
                system.Stop();
    }

    private void ShiftColor(Color newColor)
    {
        StopCoroutine(nameof(ColorShiftRoutine));

        _renderer.GetPropertyBlock(_propertyBlock);
        var currentColor = _propertyBlock.GetColor(EMISSION_COLOR);
        StartCoroutine(ColorShiftRoutine(currentColor, newColor));
    }

    private void SwitchState(State state)
    {
        _state = state;
        switch (_state)
        {
            case State.Active:
                ToggleParticleSystems(true);
                ShiftColor(activeColor);
                _uiController.DisplayLogMessage(logMessageWhenEntered, LogDisplayType.DisplayAndClear);
                _playerEventController.StatsController.ChargeRegenerationTimeModifier = regenerationSpeedMultiplier;
                break;
            
            case State.Inactive:
                ShiftColor(inactiveColor);
                break;
            
            case State.OnCooldown:
                ToggleParticleSystems(false);
                ShiftColor(cooldownColor);
                StartCoroutine(CooldownRoutine());
                _playerEventController.StatsController.ChargeRegenerationTimeModifier = 1;
                break;
        }
    }
}
