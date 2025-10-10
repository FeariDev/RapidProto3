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
    public PlayerMovement movement;
    public new PlayerCamera camera;
    public PlayerStatistics statistics;
    public PlayerInventory inventory;
    public PlayerAttack attack;
    public PlayerLevel level;
    public PlayerHealth health;



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
    }

    #endregion
}
