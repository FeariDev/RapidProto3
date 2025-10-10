
using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        private set
        {
            maxHealth = value;
        }
    }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = value;
        }
    }

    public event Action OnUpdateHealth;



    public void AddHealth(float value)
    {
        CurrentHealth += value;

        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnUpdateHealth?.Invoke();
    }
    public void TakeDamage(float amount)
    {
        AddHealth(-amount);
    }

    void CheckAliveStatus()
    {
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }



    #region Unity lifecycle

    public virtual void Awake()
    {
        OnUpdateHealth += CheckAliveStatus;

        CurrentHealth = MaxHealth;
    }

    public virtual void Start()
    {
        
    }

    #endregion
}
