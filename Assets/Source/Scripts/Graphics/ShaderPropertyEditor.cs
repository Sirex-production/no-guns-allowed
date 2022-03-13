using System;
using NaughtyAttributes;
using UnityEngine;

public enum PropertyType
{
    Int,
    Float,
    Color,
}

[Serializable]
public class ShaderProperty
{
    public string name;
    public PropertyType type;

    [ShowIf(nameof(type), PropertyType.Int)]
    [AllowNesting]
    public int intValue;

    [ShowIf(nameof(type), PropertyType.Float)]
    [AllowNesting]
    public float floatValue;

    [InfoBox("Due to the conflicts between custom and native attributes, color box will always be shown. " +
             "Ignore it if you have selected other value type.")]
    [ShowIf(nameof(type), PropertyType.Float)]
    [ColorUsage(false, true)]
    public Color color;
}

public class ShaderPropertyEditor : MonoBehaviour
{
    [SerializeField] private Renderer[] rendererReferences;
    [SerializeField] private ShaderProperty[] properties;

    private MaterialPropertyBlock _propertyBlock;

    private void OnValidate()
    {
        _propertyBlock = new MaterialPropertyBlock();

        foreach (var property in properties)
        {
            foreach (var meshRenderer in rendererReferences)
            {
                meshRenderer.GetPropertyBlock(_propertyBlock);
                SetProperty(property);
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }

    private void SetProperty(ShaderProperty property)
    {
        var propertyID = Shader.PropertyToID(property.name);

        switch (property.type)
        {
            case PropertyType.Int:
                _propertyBlock.SetInt(propertyID, property.intValue);
                break;

            case PropertyType.Float:
                _propertyBlock.SetFloat(propertyID, property.floatValue);
                break;

            case PropertyType.Color:
                _propertyBlock.SetColor(propertyID, property.color);
                break;
        }
    }  

    private void Awake()
    {
        Destroy(this);
    }
}
