using UnityEngine;
using System;
using System.Collections;

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
    [SerializeField] GameObject explosionSprite;

    [Header("Drops")]
    public GameObject xpPrefab;
    public GameObject rrpPrefab;
    [Range(0f, 1f)] public float rrpDropChance = 0.1f;

    private float currentHealth;
    private Transform player;
    private float attackTimer;

    private bool isFrozen = false;
    private float originalSpeed;
    private float originalCooldown;

    private SpriteRenderer sr;
    private Color normalColor;
    public Color frozenColor = Color.gray7;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            normalColor = sr.color;

        originalSpeed = moveSpeed;
        originalCooldown = attackCooldown;
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

        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(explodeDamage);
        }

        explosionSprite.SetActive(true);
        moveSpeed = 0;

        OnDeath?.Invoke();
        Destroy(gameObject, 0.6f);
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

    public void Freeze(float duration)
    {
        if (!isFrozen)
            StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        isFrozen = true;
        moveSpeed = 0f;
        attackCooldown = 1000f;

        if (sr != null)
            sr.color = frozenColor;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        attackCooldown = originalCooldown;
        isFrozen = false;

        if (sr != null)
            sr.color = normalColor;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerHealth ph = col.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);
        }
    }
}