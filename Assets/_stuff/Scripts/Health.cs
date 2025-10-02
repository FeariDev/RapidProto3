
using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public event Action OnUpdateHealth;



    public void UpdateHealth(float value)
    {
        currentHealth += value;

        OnUpdateHealth?.Invoke();
    }

    void CheckAliveStatus()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }



    #region Unity lifecycle

    void Awake()
    {
        OnUpdateHealth += CheckAliveStatus;

        currentHealth = maxHealth;
    }

    #endregion
}
