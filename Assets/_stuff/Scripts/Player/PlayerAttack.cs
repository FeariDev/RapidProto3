using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject slashPrefab;
    public float attackCooldown = 1f;
    public float attackDamage = 10f;
    public float slashLifetime = 0.5f;
    public float slashDistance = 1f;

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void PerformAttack()
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
}
