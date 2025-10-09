using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Different types of attacks the player can switch between
    public enum AttackType { Slash, Bullet, Chainsaw }

    [Header("Attack Settings")]
    public Weapon slashPrefab;
    public Weapon bulletPrefab;
    public Weapon chainsawPrefab;
    public Weapon kelmuPrefab;

    [Header("attackAudio")]
    [SerializeField] private AudioSource slashAudio;
    [SerializeField] private AudioSource bulletAudio;
    [SerializeField] private AudioSource chainsawAudio;
    [SerializeField] private AudioSource kelmuAudio;


    [Header("Bullet Settings")]

  
    public float chainsawCooldown = 0.2f;   

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        // Switch attack with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = slashPrefab;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = bulletPrefab;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeapon = chainsawPrefab;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            currentWeapon = kelmuPrefab;

            // Automatic attack between intervals
            float currentCooldown = currentWeapon.attackCooldown;
        if (attackTimer >= currentCooldown)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void PerformAttack()
    {
        Weapon newWeapon = Instantiate(currentWeapon, transform.position, Quaternion.identity, transform);
        newWeapon.Attack(transform.position);

        //Sound effects
        if (currentWeapon == slashPrefab)
        {
            slashAudio.Play();
        }
        else if (currentWeapon == bulletPrefab)
        {
            bulletAudio.Play();
        }
        else if (currentWeapon == chainsawPrefab)
        {
            if (!chainsawAudio.isPlaying) // only start if not already playing
            {
                chainsawAudio.loop = true;
                chainsawAudio.Play();
            }
        }
        else if (currentWeapon = kelmuPrefab)
        {
            kelmuAudio.Play();
        }
        if (currentWeapon != chainsawPrefab)
        {
            chainsawAudio.loop = false;
            chainsawAudio.Stop();
        }
    }

   


    public Weapon currentWeapon;
}