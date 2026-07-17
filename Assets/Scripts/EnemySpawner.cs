using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public GameObject enemyPrefab;
    public Terrain targetTerrain;
    public Transform player;

    public int enemiesPerNight = 3;
    public float spawnInterval = 3f;
    public int maxActiveEnemies = 6;
    public float minSpawnDistance = 15f;
    public float maxSpawnDistance = 40f;
    public bool despawnAtDay = true;

    private bool spawnedTonight;
    private Coroutine spawnRoutine;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (dayNightCycle == null)
        {
            return;
        }

        if (dayNightCycle.IsNight && !spawnedTonight)
        {
            spawnedTonight = true;
            spawnRoutine = StartCoroutine(SpawnEnemiesOverTime());
        }
        else if (!dayNightCycle.IsNight && spawnedTonight)
        {
            spawnedTonight = false;

            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }

            if (despawnAtDay)
            {
                DespawnEnemies();
            }
        }
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        for (int i = 0; i < enemiesPerNight; i++)
        {
            activeEnemies.RemoveAll(e => e == null);

            if (activeEnemies.Count < maxActiveEnemies)
            {
                SpawnOneEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOneEnemy()
    {
        if (enemyPrefab == null || targetTerrain == null || player == null)
        {
            Debug.LogWarning("EnemySpawner is missing a prefab, terrain, or player reference.");
            return;
        }

        Vector3? spawnPos = FindSpawnPosition();

        if (spawnPos.HasValue)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPos.Value, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    private Vector3? FindSpawnPosition()
    {
        int maxAttempts = 20;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
            Vector3 candidate = player.position + offset;

            TerrainData terrainData = targetTerrain.terrainData;
            Vector3 terrainPos = targetTerrain.transform.position;

            float localX = candidate.x - terrainPos.x;
            float localZ = candidate.z - terrainPos.z;

            if (localX < 0f || localX > terrainData.size.x || localZ < 0f || localZ > terrainData.size.z)
            {
                continue;
            }

            float height = targetTerrain.SampleHeight(candidate);
            candidate.y = height + terrainPos.y;

            return candidate;
        }

        Debug.LogWarning("Could not find a valid spawn position near the player.");
        return null;
    }

    private void DespawnEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();
    }
}