using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public float attackCooldown = 1f;
    public virtual void Attack(Vector3 attackPos)
    {

    }
    public float damage = 8f;
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }
}
