using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Different types of attacks the player can switch between
    public enum AttackType { Slash, Bullet, Chainsaw }

    [Header("Attack Settings")]
    public Weapon slashPrefab;
    public Weapon bulletPrefab;
    public Weapon chainsawPrefab;

     


    [Header("Bullet Settings")]

  
    public float chainsawCooldown = 0.2f;   

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        // Switch attack with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = slashPrefab;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = bulletPrefab;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeapon = chainsawPrefab;

        // Automatic attack between intervals
        float currentCooldown = currentWeapon.attackCooldown;
        if (attackTimer >= currentCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void PerformAttack()
    {
        Weapon newWeapon = Instantiate(currentWeapon, transform.position, Quaternion.identity, transform);
        newWeapon.Attack(transform.position);
    }

   


    public Weapon currentWeapon;
}