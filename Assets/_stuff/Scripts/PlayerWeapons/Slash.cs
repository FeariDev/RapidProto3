using UnityEngine;

public class Slash : Weapon
{
    InputSystem_Actions input;

    public float slashLifetime = 0.5f;
    public float slashDistance = 1f;
    public override void Attack(Vector3 attackPos)
    {
        Vector3 dir = AimHelper.GetAimDirection(transform);
        Vector3 slashPos = attackPos + dir * slashDistance;

        transform.position = attackPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, slashLifetime);
    }
}