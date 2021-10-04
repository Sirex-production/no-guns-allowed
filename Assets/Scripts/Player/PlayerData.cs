using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ingame/Player data", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Dash options")]
    [SerializeField] [Min(0)] private float dashForce = 50;
    [SerializeField] [Min(0)] private float afterDashForce = 10;
    [SerializeField] [Min(0)] private float maxDashDistance = 10;

    [Header("Charge options")] [SerializeField]
    private bool areChargesUsed = true;
    [SerializeField] [Min(0)] private int initialNumberOfCharges = 5;
    [SerializeField] [Min(0)] private float chargeRegenerationTime = 1f;

    public bool AreChargesUsed => areChargesUsed;
    public float DashForce => dashForce;
    public float AfterDashForce => afterDashForce;
    public float MaxDashDistance => maxDashDistance;

    public int InitialNumberOfCharges => initialNumberOfCharges;
    public float ChargeRegenerationTime => chargeRegenerationTime;
}
