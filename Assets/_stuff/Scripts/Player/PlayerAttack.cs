using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum AttackType { Slash, Bullet } // Different weapons/attack types
    private AttackType currentAttack = AttackType.Slash;

    [Header("Attack Settings")]
    public GameObject slashPrefab;
    public GameObject bulletPrefab;

    public float attackCooldown = 1f;
    public float attackDamage = 10f;
    public float slashLifetime = 0.5f;
    public float slashDistance = 1f;

    [Header("Bullet Settings")]
    public float bulletDamage = 5f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 6f;

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        // Switch attack with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAttack = AttackType.Slash;
            Debug.Log("Switched to: " + currentAttack);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAttack = AttackType.Bullet;
            Debug.Log("Switched to: " + currentAttack);
        }

        if (attackTimer >= attackCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void PerformAttack()
    {
        if (currentAttack == AttackType.Slash)
            PerformSlash();
        else if (currentAttack == AttackType.Bullet)
            PerformBullet();
    }

    void PerformSlash()
    {
        if (slashPrefab == null) return;

        // Get mouse pos
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Dir to mouse
        Vector3 dir = (mousePos - transform.position).normalized;

        // Pos the slash hitbox in front of player
        Vector3 slashPos = transform.position + dir * slashDistance;

        // Spawn slash
        GameObject slash = Instantiate(slashPrefab, slashPos, Quaternion.identity, transform);

        // Rotate to face mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        slash.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Set damage
        Slash slashScript = slash.GetComponent<Slash>();
        if (slashScript != null)
            slashScript.damage = attackDamage;

        Destroy(slash, slashLifetime);
    }

    void PerformBullet()
    {
        if (bulletPrefab == null) return;

        // Get mouse pos
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Dir to mouse
        Vector3 dir = (mousePos - transform.position).normalized;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Rotate bullet to face dir
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Add velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;

        // Set damage
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.damage = bulletDamage;

        Destroy(bullet, bulletLifetime);
    }
}