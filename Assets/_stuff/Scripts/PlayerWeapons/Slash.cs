using UnityEngine;

public class Slash : MonoBehaviour
{
    public float damage = 10f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }
}
