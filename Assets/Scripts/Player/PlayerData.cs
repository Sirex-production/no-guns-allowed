using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ingame/Player data", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] [Min(0)] private float dashForce;
    [SerializeField] [Range(0, 1)] private float velocityScaleFactorAfterDash;
    [SerializeField] [Min(0)] private float maximalDashDistance;

    public float DashForce => dashForce;
    public float VelocityScaleFactorAfterDash => velocityScaleFactorAfterDash;
    public float MaximalDashDistance => maximalDashDistance;
}
