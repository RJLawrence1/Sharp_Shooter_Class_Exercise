using System.Collections;
using UnityEngine;

// Put this on an empty GameObject called "EnemySpawner".
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;   // your Enemy PREFAB (from the Project window)
    [SerializeField] Transform[] spawnPoints;  // one or more SpawnPoint objects
    [SerializeField] float spawnDelay = 4f;    // seconds between spawns

    [Header("Optional: Difficulty Ramp")]
    [SerializeField] float delayDecrease = 0.2f; // how much faster spawning gets per kill
    [SerializeField] float minDelay = 1f;        // never spawn faster than this

    bool gameActive = true;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine: spawn, wait, spawn, wait... until the game ends
    IEnumerator SpawnEnemies()
    {
        while (gameActive)
        {
            // Pick a random spawn point from the array
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(enemyPrefab, point.position, point.rotation);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void StopSpawning()
    {
        gameActive = false;
    }

    // Optional extension: called by GameManager every time a robot dies
    public void IncreaseDifficulty()
    {
        spawnDelay = Mathf.Max(minDelay, spawnDelay - delayDecrease);
    }
}
