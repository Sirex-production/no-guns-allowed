using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ingame/Player data", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Dash options")]
    [SerializeField] [Min(0)] private float dashForce;
    [SerializeField] [Range(0, 1)] private float velocityScaleFactorAfterDash;
    [SerializeField] [Min(0)] private float maximalDashDistance;
    [Header("Charge options")]
    [SerializeField] [Min(0)] private int initialNumberOfCharges = 5;
    [SerializeField] [Min(0)] private float chargeRegenerationTime = 1f;

    public float DashForce => dashForce;
    public float VelocityScaleFactorAfterDash => velocityScaleFactorAfterDash;
    public float MaximalDashDistance => maximalDashDistance;

    public int InitialNumberOfCharges => initialNumberOfCharges;
    public float ChargeRegenerationTime => chargeRegenerationTime;
}
