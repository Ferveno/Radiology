using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class EnemyPrefabProbability
    {
        public GameObject prefab;
        public float spawnProbability;
    }

    public List<EnemyPrefabProbability> enemies = new List<EnemyPrefabProbability>();
    private Dictionary<GameObject, Queue<GameObject>> pooledEnemies = new Dictionary<GameObject, Queue<GameObject>>();

    void Start()
    {
        foreach (var enemy in enemies)
        {
            pooledEnemies.Add(enemy.prefab, new Queue<GameObject>());
            // Initialize the pooled enemies for each enemy type
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(enemy.prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                pooledEnemies[enemy.prefab].Enqueue(obj);
            }
        }
    }

    public int poolSize = 20; // Fixed pool size for each enemy type

    public GameObject GetPooledEnemy()
    {
        // Select a random enemy type based on their probabilities
        float totalProbability = 0f;
        foreach (var enemy in enemies)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability;

        float accumulatedProbability = 0f;
        foreach (var enemy in enemies)
        {
            accumulatedProbability += enemy.spawnProbability;
            if (randomPoint <= accumulatedProbability)
            {
                return GetEnemyFromPool(enemy.prefab);
            }
        }

        // If no enemy is selected, return null
        return null;
    }

    private GameObject GetEnemyFromPool(GameObject prefab)
    {
        if (pooledEnemies.ContainsKey(prefab) && pooledEnemies[prefab].Count > 0)
        {
            return pooledEnemies[prefab].Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            return obj;
        }
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        foreach (var item in pooledEnemies)
        {
            if (item.Key == enemy.gameObject)
            {
                pooledEnemies[item.Key].Enqueue(enemy);
                return;
            }
        }
        Destroy(enemy);
    }
}
