using Ingame;
using Ingame.Graphics;
using UnityEngine;
using Zenject;

public class EnvironmnetShakeInvokable : MonoInvokable
{
    [SerializeField] private float screenShakeDuration = 1f;
    
    [Inject] private EffectsManager _effectsManager;

    public override void Invoke()
    {
        _effectsManager.ShakeEnvironment(screenShakeDuration);
        
        base.Invoke();
    }
}
