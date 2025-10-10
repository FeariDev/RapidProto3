using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner spawner;
    public GameObject[] allEnemyPrefabs;
    public float timeBetweenWaves = 5f;
    public int maxWaves = 50;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;

    private int currentWave = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        if (spawner == null)
            Debug.LogError("WaveManager: Spawner not assigned");
        if (allEnemyPrefabs.Length == 0)
            Debug.LogError("WaveManager: No enemy prefabs assigned");

        StartCoroutine(HandleWaves());
    }

    void Update()
    {
        waveText.text = string.Format("Wave {0}", currentWave);
        enemiesLeftText.text = string.Format("Enemies Left: {0}", aliveEnemies.Count);
    }

    IEnumerator HandleWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;
            yield return StartCoroutine(SpawnWave(currentWave));

            yield return new WaitUntil(() => aliveEnemies.Count == 0);

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        int enemiesToSpawn = Mathf.RoundToInt(Mathf.Lerp(10, 200, (float)waveNumber / maxWaves));

        for (int i=0; i<enemiesToSpawn; i++)
        {
            GameObject enemyPrefab = ChooseEnemyType(waveNumber);

            GameObject enemy = spawner.SpawnEnemy(enemyPrefab);
            aliveEnemies.Add(enemy);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnDeath += () =>
                {
                    aliveEnemies.Remove(enemy);
                };
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    GameObject ChooseEnemyType(int waveNumber)
    {
        int unlockedTypes = Mathf.Min(1 + waveNumber / 5, allEnemyPrefabs.Length);

        List<GameObject> candidates = new List<GameObject>();

        for (int i=0; i<unlockedTypes; i++)
        {
            if (i==0 && waveNumber >= 30) continue;

            int weight = 1;
            if (i==0) weight = 4;
            if (i==1) weight = 3;
            if (i==2) weight = 2;
            if (i==3) weight = 2;
            if (i==4) weight = 3;

            for (int w=0; w<weight; w++)
                candidates.Add(allEnemyPrefabs[i]);
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

	[Button]
	void DebugKillEnemies()
	{
		foreach (GameObject enemy in aliveEnemies)
		{
			Destroy(enemy);
		}

		aliveEnemies.Clear();
	}
}