using UnityEngine;

public class EmissionMapEditor : MonoBehaviour
{
    [SerializeField] private Renderer[] rendererReferences;
    [SerializeField] [ColorUsage(false, true)] private Color emissionMapColor;

    private static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");

    private MaterialPropertyBlock _propertyBlock;

    private void OnValidate()
    {
        _propertyBlock = new MaterialPropertyBlock();

        foreach (var meshRenderer in rendererReferences)
        {
            meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(EMISSION_COLOR, emissionMapColor);
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }


    private void Awake()
    {
        Destroy(this);
    }
}
