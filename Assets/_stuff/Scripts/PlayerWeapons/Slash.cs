using UnityEngine;

public class Slash : Weapon
{
    public float attackDamage = 10f;
    public float slashLifetime = 0.5f;
    public float slashDistance = 1f;
    public override void Attack(Vector3 attackPos)
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - attackPos).normalized;

        Vector3 slashPos = attackPos + dir * slashDistance;
        //GameObject slash = Instantiate(WeaponPrefab, slashPos, Quaternion.identity, transform);

        transform.position = attackPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log(gameObject);
        Destroy(gameObject ,slashLifetime);
    }
}
