using System.Collections;
using UnityEngine;

public class EnemyDetectionSystem : MonoBehaviour
{
    public static EnemyDetectionSystem instance;

    public float detectionRadius = 10f; // Radius within which enemies are detected
    public LayerMask enemyLayer; // Layer mask to specify which layer the enemies are on

    private float nextTimeToFire = 0f;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private float muzzleDelay;
    [SerializeField] private SpriteRenderer muzzleRef;

    public GameObject[] guns;
    public Transform riffleFirePoint;
    public Transform pistolFirePoint;
    public Transform[] shotGunFirePoints;

    [SerializeField] private SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer
    public float fireRate = 1f; // Default fire rate

    Vector3 newAim;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Detect enemies within the detection radius
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // Find the closest enemy
            Transform closestEnemy = GetClosestEnemy(enemiesInRange);

            // Aim the gun towards the closest enemy
            AimAtTarget(closestEnemy.position);

            if (closestEnemy.gameObject.tag=="Ghost" || closestEnemy.gameObject.tag=="Boss")
            {
                newAim = new Vector3(closestEnemy.position.x, closestEnemy.position.y + 1.5f, closestEnemy.position.z);

                AimAtTarget(newAim);
            }

            // Flip the player to face the enemy
            FlipPlayer(closestEnemy.position);

            // Shoot at the enemy if it's time to fire
            if (Time.time >= nextTimeToFire)
            {
                Shoot();
            }
        }
        else
        {
            AimAtTarget(Vector3.zero);
        }
    }

    Transform GetClosestEnemy(Collider2D[] enemies)
    {
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

    void AimAtTarget(Vector3 targetPosition)
    {
        GetComponent<PlayerAimWeapon>().HandleAiming(targetPosition);
    }

    void FlipPlayer(Vector3 targetPosition)
    {
        // Determine if the enemy is to the left or right of the player
        if (targetPosition.x < transform.position.x)
        {
            playerSprite.flipX = true; // Enemy is to the left
        }
        else
        {
            playerSprite.flipX = false; // Enemy is to the right
        }
    }

    void Shoot()
    {
        // if (!GameManager.Instance.isFlameThrower)
        {
            StartCoroutine(MuzzleFlash());

            // Get a bullet from the pool
            GameObject bullet = BulletPool.Instance.GetBullet();
            for (int i = 0; i < guns.Length; i++)
            {
                if (guns[i].gameObject.activeInHierarchy)
                {
                   

                    if (guns[i].CompareTag("Pistol"))
                    {
                        fireRate = 4f; // Example fire rate for Pistol
                        pistolFirePoint.gameObject.SetActive(true);
                        bullet.transform.position = pistolFirePoint.transform.position;
                        bullet.transform.rotation = pistolFirePoint.transform.rotation;
                    }
                    else if (guns[i].CompareTag("Riffle"))
                    {
                        fireRate = 8f; // Example fire rate for Riffle
                        riffleFirePoint.gameObject.SetActive(true);
                        bullet.transform.position = riffleFirePoint.transform.position;
                        bullet.transform.rotation = riffleFirePoint.transform.rotation;
                    }
                    else if (guns[i].CompareTag("ShotGun"))
                    {
                        fireRate = 4f; // Example fire rate for ShotGun
                        GameObject bullet1 = BulletPool.Instance.GetBullet();
                        GameObject bullet2 = BulletPool.Instance.GetBullet();

                        for (int j = 0; j < shotGunFirePoints.Length; j++)
                        {
                            shotGunFirePoints[j].gameObject.SetActive(true);
                        }
                        bullet.transform.position = shotGunFirePoints[0].transform.position;
                        bullet.transform.rotation = shotGunFirePoints[0].transform.rotation;

                        bullet1.transform.position = shotGunFirePoints[1].transform.position;
                        bullet1.transform.rotation = shotGunFirePoints[1].transform.rotation;

                        bullet2.transform.position = shotGunFirePoints[2].transform.position;
                        bullet2.transform.rotation = shotGunFirePoints[2].transform.rotation;
                    }

                    // Set next time to fire based on the specific gun's fire rate
                    nextTimeToFire = Time.time + 1f / fireRate;
                }
            }
        }

        // Play shooting sound or animation here if necessary
        Debug.Log("Shoot!");
    }

    IEnumerator MuzzleFlash()
    {
        muzzleRef.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(muzzleDelay);
        muzzleRef.color = new Color(1f, 1f, 1f, 0f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
