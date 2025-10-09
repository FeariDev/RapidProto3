using UnityEngine;
using System.Collections.Generic;

public class Kelmu : Weapon
{
	[Header("Kelmu Settings")]
	public float bulletDamage = 20f;
	public float bulletSpeed = 10f;
	public float bulletLifetime = 3f;
	public float freezeDuration = 2f;
	public int pierceCount = 5;

	private int enemiesHit = 0;
	
	public override void Attack(Vector3 attackPos)
	{

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		Vector3 dir = (mousePos - transform.position).normalized;


		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);

		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
			rb.linearVelocity = dir * bulletSpeed;

		Destroy(gameObject, bulletLifetime);
	}

	private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
	
	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			Enemy enemy = col.GetComponent<Enemy>();
			if (enemy != null && !hitEnemies.Contains(enemy))
			{
				hitEnemies.Add(enemy);
				enemy.TakeDamage(damage);
				enemy.Freeze(freezeDuration);
				enemiesHit++;
				
				if (enemiesHit >= pierceCount)
					Destroy(gameObject);
			}
		}
	}
}
