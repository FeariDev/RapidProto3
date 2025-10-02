using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public event Action OnDeath;

    [Header("General Stats")]
    public float maxHealth = 10f;
    public float moveSpeed = 3f;
    public float damage = 2f;
    public bool isRanged = false;
    public bool isCreeper = false;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float attackCooldown = 1.5f;
    public float attackRange = 8f;
    public float projectileSpeed = 10f;

    [Header("Creeper Settings")]
    public float explodeRange = 1.5f;
    public float explodeDamage = 10f;

    [Header("Drops")]
    public GameObject xpPrefab;
    public GameObject rrpPrefab;
    [Range(0f, 1f)] public float rrpDropChance = 0.1f;

    private float currentHealth;
    private Transform player;
    private float attackTimer;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isCreeper && distance <= explodeRange)
        {
            Explode();
            return;
        }

        if (isRanged)
            HandleRangedEnemy(distance);
        else
            HandleMeleeEnemy();
    }

    void HandleMeleeEnemy()
    {
        MoveTowards(player.position);
    }

    void HandleRangedEnemy(float distance)
    {
        if (distance > attackRange * 0.9f)
            MoveTowards(player.position);
        else if (distance < attackRange * 0.5f)
            MoveAway(player.position);

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            ShootProjectile();
            attackTimer = 0f;
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void MoveAway(Vector3 target)
    {
        Vector3 dir = (transform.position - target).normalized;
        transform.position += dir * (moveSpeed * 0.75f) * Time.deltaTime;
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * projectileSpeed;
    }

    void Explode()
    {
        Debug.Log($"{gameObject.name} exploded");

        // TODO: Damage player
        // e.g.: player.GetComponent<PlayerHealth>().TakeDamage(explodeDamage);

        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        DropLoot();

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        if (xpPrefab != null)
            Instantiate(xpPrefab, transform.position, Quaternion.identity);

        if (rrpPrefab != null && UnityEngine.Random.value <= rrpDropChance)
            Instantiate(rrpPrefab, transform.position, Quaternion.identity);
    }
}