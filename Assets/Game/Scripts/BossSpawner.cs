using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner Instance;
    [HideInInspector]
    public GameObject player;
    private Camera mainCamera;

    public int enemyKillCount;
    public int bossKillCount;
    [SerializeField] private GameObject SubBoss_1;
    [SerializeField] private GameObject SubBoss_2;
    [SerializeField] private GameObject MainBoss;

    // Boundary margin offset for adjustment
    public Vector2 boundaryMarginOffset = new Vector2(2f, 2f);

    // Fixed boundaries for spawning
    private Vector3 fixedScreenBottomLeft;
    private Vector3 fixedScreenTopRight;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
    }

    public void UpdateEnemyKillCounter()
    {
        enemyKillCount++;


        if (SceneManager.GetActiveScene().name.Equals("World 1"))
        {
            if (enemyKillCount == 20)
            {
                GameObject subBoss = Instantiate(SubBoss_1);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 40)
            {
                GameObject subBoss = Instantiate(SubBoss_2);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 60 && !SubBoss_1.activeInHierarchy && !SubBoss_2.activeInHierarchy)
            {
                GameObject subBoss = Instantiate(MainBoss);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
        }
        else if (SceneManager.GetActiveScene().name.Equals("World 2"))
        {
            if (enemyKillCount == 30)
            {
                GameObject subBoss = Instantiate(SubBoss_1);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 60)
            {
                GameObject subBoss = Instantiate(SubBoss_2);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 90 && !SubBoss_1.activeInHierarchy && !SubBoss_2.activeInHierarchy)
            {
                GameObject subBoss = Instantiate(MainBoss);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
        }
        else if (SceneManager.GetActiveScene().name.Equals("World 3"))
        {
            if (enemyKillCount == 60)
            {
                GameObject subBoss = Instantiate(SubBoss_1);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 90)
            {
                GameObject subBoss = Instantiate(SubBoss_2);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
            }
            else if (enemyKillCount == 120 && !SubBoss_1.activeInHierarchy && !SubBoss_2.activeInHierarchy)
            {
                GameObject subBoss = Instantiate(MainBoss);
                subBoss.transform.position = GetRandomSpawnPositionOutsideView();
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
