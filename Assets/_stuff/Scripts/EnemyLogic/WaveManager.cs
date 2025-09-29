using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner spawner;
    public GameObject[] allEnemyPrefabs;
    public float timeBetweenWaves = 5f;
    public int maxWaves = 50;

    private int currentWave = 0;

    void Start()
    {
        if (spawner == null)
            Debug.LogError("WaveManager: Spawner not assigned");
        if (allEnemyPrefabs.Length == 0)
            Debug.LogError("WaveManager: No enemy prefabs assigned");

        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;
            yield return StartCoroutine(SpawnWave(currentWave));

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves completed!");
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        Debug.Log("Starting wave " + waveNumber);

        int unlockedTypes = Mathf.Min(1 + waveNumber / 5, allEnemyPrefabs.Length);

        int enemiesToSpawn = 5 + waveNumber * 2;

        for (int i=0; i < enemiesToSpawn; i++)
        {
            GameObject enemyPrefab = allEnemyPrefabs[Random.Range(0, unlockedTypes)];

            spawner.SpawnEnemy(enemyPrefab);

            yield return new WaitForSeconds(0.2f);
        }
    }
}