using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Different types of attacks the player can switch between
    public enum AttackType { Slash, Bullet, Chainsaw }
    private AttackType currentAttack = AttackType.Slash;

    [Header("Attack Settings")]
    public GameObject slashPrefab;
    public GameObject bulletPrefab;
    public GameObject chainsawPrefab;

    public float attackCooldown = 1f;     
    public float attackDamage = 10f;
    public float slashLifetime = 0.5f;
    public float slashDistance = 1f;

    [Header("Bullet Settings")]
    public float bulletDamage = 20f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;

    [Header("Chainsaw Settings")]
    public float chainsawDamage = 8f;
    public float chainsawLifetime = 0.3f;   
    public float chainsawDistance = 0.5f;    
    public float chainsawCooldown = 0.2f;   

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        // Switch attack with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentAttack = AttackType.Slash;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentAttack = AttackType.Bullet;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentAttack = AttackType.Chainsaw;

        // Automatic attack between intervals
        float currentCooldown = GetCurrentCooldown();
        if (attackTimer >= currentCooldown)
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
        else if (currentAttack == AttackType.Chainsaw)
            PerformChainsaw();
    }

    float GetCurrentCooldown()
    {
        switch (currentAttack)
        {
            case AttackType.Chainsaw: return chainsawCooldown;
            case AttackType.Bullet: return attackCooldown;
            case AttackType.Slash: return attackCooldown;
            default: return attackCooldown;
        }
    }

    void PerformSlash()
    {
        if (slashPrefab == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - transform.position).normalized;

        Vector3 slashPos = transform.position + dir * slashDistance;
        GameObject slash = Instantiate(slashPrefab, slashPos, Quaternion.identity, transform);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        slash.transform.rotation = Quaternion.Euler(0, 0, angle);

        Slash slashScript = slash.GetComponent<Slash>();
        if (slashScript != null)
            slashScript.damage = attackDamage;

        Destroy(slash, slashLifetime);
    }

    void PerformBullet()
    {
        if (bulletPrefab == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.damage = bulletDamage;

        Destroy(bullet, bulletLifetime);
    }

    void PerformChainsaw()
    {
        if (chainsawPrefab == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - transform.position).normalized;

        Vector3 chainsawPos = transform.position + dir * chainsawDistance;
        GameObject chainsaw = Instantiate(chainsawPrefab, chainsawPos, Quaternion.identity, transform);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        chainsaw.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Chainsaw has its own script to handle damage
        Chainsaw chainsawScript = chainsaw.GetComponent<Chainsaw>();
        if (chainsawScript != null)
            chainsawScript.damage = chainsawDamage;

        Destroy(chainsaw, chainsawLifetime);
    }
}