using UnityEngine;

public class PlayerHealth : Health
{
    public override void Die()
    {
        Debug.Log("YOU DIED!");
    }
}
