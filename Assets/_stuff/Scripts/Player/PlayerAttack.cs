using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public enum AttackType { Slash, Bullet, Chainsaw, Kelmu }

    [Header("Attack Settings")]
    public Weapon slashPrefab;
    public Weapon bulletPrefab;
    public Weapon chainsawPrefab;
    public Weapon kelmuPrefab;

    [Header("Attack Audio")]
    [SerializeField] private AudioSource slashAudio;
    [SerializeField] private AudioSource bulletAudio;
    [SerializeField] private AudioSource chainsawAudio;
    [SerializeField] private AudioSource kelmuAudio;

    [Header("Cooldowns")]
    public float chainsawCooldown = 0.2f;

    private float attackTimer;
    public Weapon currentWeapon;
    private int currentWeaponIndex = 0;

    private Weapon[] weaponList;

    public Action<int> OnWeaponSwitched;

    void Start()
    {
        // Put all weapon prefabs in order
        weaponList = new Weapon[] { slashPrefab, bulletPrefab, chainsawPrefab, kelmuPrefab };

        // Default to first weapon
        if (weaponList.Length > 0)
        {
            currentWeapon = weaponList[0];
            OnWeaponSwitched?.Invoke(1);
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        // ✅ Keyboard number key switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);

        // ✅ Controller RB cycling (Right Bumper)
        if (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            CycleNextWeapon();
        }

        // Automatic attack loop
        if (currentWeapon == null) return;
        float currentCooldown = currentWeapon.cooldown;
        if (attackTimer >= currentCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weaponList.Length) return;

        currentWeaponIndex = index;
        currentWeapon = weaponList[index];
        OnWeaponSwitched?.Invoke(index + 1);
    }

    void CycleNextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weaponList.Length)
            currentWeaponIndex = 0;

        currentWeapon = weaponList[currentWeaponIndex];
        OnWeaponSwitched?.Invoke(currentWeaponIndex + 1);
    }

    void PerformAttack()
    {
        if (currentWeapon == null) return;

        Weapon newWeapon = Instantiate(currentWeapon, transform.position, Quaternion.identity, transform);
        newWeapon.Attack(transform.position);

        // ✅ Play weapon sounds
        if (currentWeapon == slashPrefab)
        {
            slashAudio.Play();
        }
        else if (currentWeapon == bulletPrefab)
        {
            bulletAudio.Play();
        }
        else if (currentWeapon == chainsawPrefab)
        {
            if (!chainsawAudio.isPlaying)
            {
                chainsawAudio.loop = true;
                chainsawAudio.Play();
            }
        }
        else if (currentWeapon == kelmuPrefab)
        {
            kelmuAudio.Play();
        }

        // Stop chainsaw audio if using another weapon
        if (currentWeapon != chainsawPrefab)
        {
            chainsawAudio.loop = false;
            chainsawAudio.Stop();
        }
    }
}