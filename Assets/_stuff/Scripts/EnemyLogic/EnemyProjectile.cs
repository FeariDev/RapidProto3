using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 2f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // TODO: Damage player
            // e.g.: col.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (col.CompareTag("Wall"))
            Destroy(gameObject);
    }
}