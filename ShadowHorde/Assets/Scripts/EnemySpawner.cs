using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Wave Settings")]
    public float spawnRadius = 12f;
    public float baseSpawnInterval = 1.5f;
    public int baseEnemiesPerWave = 8;
    public float waveInterval = 20f;       // seconds between waves
    public float difficultyScalePerWave = 0.15f; // +15% harder per wave

    int waveNumber = 0;

    void Start() => StartCoroutine(WaveLoop());

    IEnumerator WaveLoop()
    {
        while (true)
        {
            waveNumber++;
            int count = baseEnemiesPerWave + waveNumber * 3;
            float interval = Mathf.Max(0.3f, baseSpawnInterval - waveNumber * 0.05f);

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(waveInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject e = Instantiate(prefab, pos, Quaternion.identity);

        float scale = 1f + waveNumber * difficultyScalePerWave;
        e.GetComponent<EnemyAI>()?.ScaleStats(scale, Mathf.Sqrt(scale));
    }
}