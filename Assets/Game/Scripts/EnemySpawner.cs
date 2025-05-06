using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public float spawnProbability; // probability weight for this enemy
    }

    public List<EnemyType> enemies = new List<EnemyType>(); // List of enemies with probabilities
    public float initialSpawnInterval = 5f; // Initial time between spawns
    public float difficultyIncreaseInterval; // Time interval to increase difficulty
    public float spawnIntervalDecrease = 0.5f; // Amount to decrease spawn interval each difficulty increase
    public float minSpawnInterval = 1f; // Minimum spawn interval

    private float nextSpawnTime;
    private float currentSpawnInterval;

    private GameObject player;
    private Camera mainCamera;

    // Boundary margin offset for adjustment
    public Vector2 boundaryMarginOffset = new Vector2(2f, 2f);

    // Fixed boundaries for spawning
    private Vector3 fixedScreenBottomLeft;
    private Vector3 fixedScreenTopRight;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        nextSpawnTime = Time.time + currentSpawnInterval;
        StartCoroutine(IncreaseDifficultyOverTime());

        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;

        // Calculate fixed screen boundaries in world coordinates
        fixedScreenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        fixedScreenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentSpawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Calculate total probability of all enemy types
        float totalProbability = 0f;
        foreach (var enemyType in enemies)
        {
            totalProbability += enemyType.spawnProbability;
        }

        // Generate a random point within the total probability range
        float randomPoint = Random.value * totalProbability;

        // Iterate through each enemy type to determine which one to spawn
        foreach (var enemyType in enemies)
        {
            // If the random point falls within the probability range of this enemy type
            if (randomPoint < enemyType.spawnProbability)
            {
                // Get an enemy from the pool
                GameObject enemyInstance = EnemyPool.Instance.GetPooledEnemy();
                if (enemyInstance != null)
                {
                    // Generate a random position outside the player's view
                    Vector3 spawnPos = GetRandomSpawnPositionOutsideView();

                    // Set the position and activate the enemy
                    enemyInstance.transform.position = spawnPos;
                    enemyInstance.SetActive(true);

                    // Get the Enemy script attached to the instantiated prefab
                    Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        // Set a random speed between 8 and 12
                        enemyScript.speed = Random.Range(8f, 12f);
                    }
                }
                return;
            }
            else
            {
                // Subtract the probability of this enemy type from the random point
                randomPoint -= enemyType.spawnProbability;
            }
        }
    }

    Vector3 GetRandomSpawnPositionOutsideView()
    {
        Vector3 spawnPos = Vector3.zero;

        bool positionFound = false;
        while (!positionFound)
        {
            // Choose a random edge to spawn from
            int edge = Random.Range(0, 4);
            float x = 0f, y = 0f;

            switch (edge)
            {
                case 0: // Left
                    x = fixedScreenBottomLeft.x - boundaryMarginOffset.x;
                    y = Random.Range(fixedScreenBottomLeft.y - boundaryMarginOffset.y, fixedScreenTopRight.y + boundaryMarginOffset.y);
                    break;
                case 1: // Right
                    x = fixedScreenTopRight.x + boundaryMarginOffset.x;
                    y = Random.Range(fixedScreenBottomLeft.y - boundaryMarginOffset.y, fixedScreenTopRight.y + boundaryMarginOffset.y);
                    break;
                case 2: // Bottom
                    x = Random.Range(fixedScreenBottomLeft.x - boundaryMarginOffset.x, fixedScreenTopRight.x + boundaryMarginOffset.x);
                    y = fixedScreenBottomLeft.y - boundaryMarginOffset.y;
                    break;
                case 3: // Top
                    x = Random.Range(fixedScreenBottomLeft.x - boundaryMarginOffset.x, fixedScreenTopRight.x + boundaryMarginOffset.x);
                    y = fixedScreenTopRight.y + boundaryMarginOffset.y;
                    break;
            }

            spawnPos = new Vector3(x, y, 0); // Ensure the z-axis is zero
            positionFound = true;
        }

        return spawnPos;
    }

    IEnumerator IncreaseDifficultyOverTime()
    {
        while (currentSpawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (mainCamera != null)
        {
            Vector3 extendedBottomLeft = new Vector3(fixedScreenBottomLeft.x - boundaryMarginOffset.x, fixedScreenBottomLeft.y - boundaryMarginOffset.y, fixedScreenBottomLeft.z);
            Vector3 extendedTopRight = new Vector3(fixedScreenTopRight.x + boundaryMarginOffset.x, fixedScreenTopRight.y + boundaryMarginOffset.y, fixedScreenTopRight.z);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(extendedBottomLeft, new Vector3(extendedTopRight.x, extendedBottomLeft.y, extendedBottomLeft.z));
            Gizmos.DrawLine(new Vector3(extendedTopRight.x, extendedBottomLeft.y, extendedBottomLeft.z), extendedTopRight);
            Gizmos.DrawLine(extendedTopRight, new Vector3(extendedBottomLeft.x, extendedTopRight.y, extendedBottomLeft.z));
            Gizmos.DrawLine(new Vector3(extendedBottomLeft.x, extendedTopRight.y, extendedBottomLeft.z), extendedBottomLeft);
        }
    }
}
