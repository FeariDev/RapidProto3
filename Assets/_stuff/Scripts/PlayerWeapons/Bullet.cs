using UnityEngine;

public class Bullet : Weapon
{

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    public override void Attack(Vector3 attackPos)
    {
        Vector3 dir = AimHelper.GetAimDirection(transform);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;

        Destroy(gameObject, bulletLifetime);
    }
}