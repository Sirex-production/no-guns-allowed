using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ingame/Player data", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Dash options")]
    [SerializeField] [Min(0)] private float dashForce;
    [SerializeField] [Min(0)] private float dashDuration;
    [Header("Charge options")]
    [SerializeField] [Min(0)] private int initialNumberOfCharges = 5;
    [SerializeField] [Min(0)] private float chargeRegenerationTime = 1f;

    public float DashForce => dashForce;
    public float DashDuration => dashDuration;

    public int InitialNumberOfCharges => initialNumberOfCharges;
    public float ChargeRegenerationTime => chargeRegenerationTime;
}
