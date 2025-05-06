using UnityEngine;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject[] powerupPrefabs; // Array of powerup prefabs to spawn
    public int initialPowerupCount = 5; // Initial number of powerups to spawn
    public float relocationTime = 10f;  // Time after which powerups relocate if not collected
    public Vector2 boundaryTopLeft;     // Top-left boundary for spawning
    public Vector2 boundaryBottomRight; // Bottom-right boundary for spawning

    private Transform player;
    private List<GameObject> activePowerups = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnInitialPowerups();
    }

    void SpawnInitialPowerups()
    {
        for (int i = 0; i < initialPowerupCount; i++)
        {
            SpawnPowerup();
        }
    }

    public void RelocatePowerup(GameObject powerup)
    {
        Vector2 newPosition = GetSpawnPosition();
        powerup.transform.position = newPosition;
    }

    void SpawnPowerup()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        GameObject powerup = Instantiate(GetRandomPowerup(), spawnPosition, Quaternion.identity);
        powerup.GetComponent<Powerup>().Initialize(this, relocationTime);
        activePowerups.Add(powerup);
    }

    Vector2 GetSpawnPosition()
    {
        float spawnX = Random.Range(boundaryTopLeft.x, boundaryBottomRight.x);
        float spawnY = Random.Range(boundaryBottomRight.y, boundaryTopLeft.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        return spawnPosition;
    }

    GameObject GetRandomPowerup()
    {
        int index = Random.Range(0, powerupPrefabs.Length);
        return powerupPrefabs[index];
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, (boundaryTopLeft - boundaryBottomRight).magnitude / 2);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(boundaryTopLeft.x, boundaryTopLeft.y, 0), new Vector3(boundaryBottomRight.x, boundaryTopLeft.y, 0));
        Gizmos.DrawLine(new Vector3(boundaryBottomRight.x, boundaryTopLeft.y, 0), new Vector3(boundaryBottomRight.x, boundaryBottomRight.y, 0));
        Gizmos.DrawLine(new Vector3(boundaryBottomRight.x, boundaryBottomRight.y, 0), new Vector3(boundaryTopLeft.x, boundaryBottomRight.y, 0));
        Gizmos.DrawLine(new Vector3(boundaryTopLeft.x, boundaryBottomRight.y, 0), new Vector3(boundaryTopLeft.x, boundaryTopLeft.y, 0));
    }
}
