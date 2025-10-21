using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerStatistics))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerLevel))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : Singleton<Player>
{
    public InputSystem_Actions input;

    public PlayerMovement movement;
    public new PlayerCamera camera;
    public PlayerStatistics statistics;
    public PlayerInventory inventory;
    public PlayerAttack attack;
    public PlayerLevel level;
    public PlayerHealth health;

    // 🧭 Add this property for right-stick aim
    public Vector2 AimDirection { get; private set; }

    #region Unity lifecycle

    protected override void Awake()
    {
        base.Awake();

        movement = GetComponent<PlayerMovement>();
        camera = GetComponent<PlayerCamera>();
        statistics = GetComponent<PlayerStatistics>();
        inventory = GetComponent<PlayerInventory>();
        attack = GetComponent<PlayerAttack>();
        level = GetComponent<PlayerLevel>();
        health = GetComponent<PlayerHealth>();

        input = new InputSystem_Actions();
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Update()
    {
        UpdateAimDirection();
    }

    #endregion

    // 🎮 Keeps AimDirection updated from right stick
    private void UpdateAimDirection()
    {
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();

        // Deadzone (to avoid flicker)
        if (lookInput.magnitude > 0.2f)
            AimDirection = lookInput.normalized;
        else
            AimDirection = Vector2.zero;
    }
}