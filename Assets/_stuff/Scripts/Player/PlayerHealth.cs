using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [Header("UI")]
    [SerializeField] Slider healthSlider;



    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("YOU DIED!");
    }

    void UpdateUI()
    {
        healthSlider.value = CurrentHealth / MaxHealth;
    }



    public float debugHealthValue;
    [Button]
    void DebugAddHealth()
    {
        AddHealth(debugHealthValue);
    }



    #region Unity lifecycle

    public override void Awake()
    {
        base.Awake();

        OnUpdateHealth += UpdateUI;
    }

    public override void Start()
    {
        base.Start();

        UpdateUI();
    }

    #endregion
}
