using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { XP, RRP }
    public CollectibleType type;

    public float amount;
    public float lifetime = 20f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (type == CollectibleType.XP)
            {
                Debug.Log("Player collected XP orb");
                Player.Instance.level.UpdateExperience(amount);
            }
            else if (type == CollectibleType.RRP)
            {
                // Example give reroll point
                // col.GetComponent<PlayerStats>().GainRRP(1);
                Debug.Log("Player collected RRP shard");
            }

            Destroy(gameObject);
        }
    }
}
