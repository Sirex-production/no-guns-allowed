using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ingame/Player data", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Lifetime options")]
    [SerializeField] [Min(0)] private float initialHp = 1;
    [SerializeField] [Min(0)] private float damage = 1f;

    [Header("Dash options")] 
    [SerializeField] private bool stopDashWhenCollidingWithEnvironment = false;
    [SerializeField] [Min(0)] private float dashForce = 50;
    [SerializeField] [Min(0)] private float afterDashForce = 10;
    [SerializeField] [Min(0)] private float maxDashDistance = 10;

    [Header("Charge options")] [SerializeField]
    private bool areChargesUsed = true;
    [SerializeField] [Min(0)] private int initialNumberOfCharges = 5;
    [SerializeField] [Min(0)] private float chargeRegenerationTime = 1f;

    public float InitialHp => initialHp;
    public float Damage => damage;
    
    public bool StopDashWhenCollidingWithEnvironment => stopDashWhenCollidingWithEnvironment;
    public float DashForce => dashForce;
    public float AfterDashForce => afterDashForce;
    public float MaxDashDistance => maxDashDistance;

    public bool AreChargesUsed => areChargesUsed;
    public int InitialNumberOfCharges => initialNumberOfCharges;
    public float ChargeRegenerationTime => chargeRegenerationTime;
}