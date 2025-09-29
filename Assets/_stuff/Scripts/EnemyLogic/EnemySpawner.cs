using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Transform player;
	public float spawnRadius = 15f;

	public void SpawnEnemy(GameObject enemyPrefab)
	{
		if (player == null || enemyPrefab == null)
		{
			Debug.LogWarning("EnemySpawner: Missing player or enemy prefab");
		}

		Vector2 spawnDir = Random.insideUnitCircle.normalized * spawnRadius;
		Vector3 spawnPos = player.position + new Vector3(spawnDir.x, spawnDir.y, 0);

		Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
	}
}