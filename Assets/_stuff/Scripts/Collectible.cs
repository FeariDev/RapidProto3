using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType { XP, RRP }
    public CollectibleType type;

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
                // Example give XP
                // col.GetComponent<PlayerStats>().GainXP(1);
                Debug.Log("Player collected XP orb");
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
