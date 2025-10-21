using UnityEngine;

public class Chainsaw : Weapon
{
    public float chainsawLifetime = 0.3f;
    public float chainsawDistance = 0.5f;
    public override void Attack(Vector3 attackPos)
    {
        Vector3 dir = AimHelper.GetAimDirection(transform);
        Vector3 chainsawPos = transform.position + dir * chainsawDistance;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, chainsawLifetime);
    }
}
