using Ingame;
using UnityEngine;

public class InvokableColorChanger : MonoInvokable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private string propertyColorName;
    [SerializeField] [ColorUsage(true, true)] private Color color;
    
    public override void Invoke()
    {
        ChangeColor();
        base.Invoke();
    }

    private void ChangeColor()
    {
        var propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        
        propertyBlock.SetColor(Shader.PropertyToID(propertyColorName), color);
        
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}
