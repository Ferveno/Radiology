using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float originalSpeed = 3f;
    public float speed = 3f; // Speed of the enemy

    private Transform playerTransform;

    [SerializeField] private int health = 3;
    public int originalHealth = 3;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int damageOnPlayer = 1;
    bool isDead = false;

    private BoxCollider2D boxCollider;

    public float stoppingDistance = 1.5f;

    [SerializeField] private bool isSpider = false; // Add a flag to check if the enemy is a spider

    private void Awake()
    {
        // Cache references to components in Awake to avoid multiple GetComponent calls
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        // Reset enemy state
        speed = originalSpeed;
        health = originalHealth;
        isDead = false;
        animator.SetBool("isDead", false);
        animator.SetBool("isHit", false);
        boxCollider.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null && !isDead)
        {
            // Calculate the direction to the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > stoppingDistance)
            {
                // Rotate the spider towards the player if it's a spider
                if (isSpider)
                {
                    RotateTowardsDirection(direction);
                }

                // Move the enemy towards the player
                transform.position += (Vector3)direction * speed * Time.deltaTime;

                // Make the enemy face the player if it's not a spider
                if (!isSpider)
                {
                    FacePlayer(direction);
                }
            }
            else
            {
                // Attack the player when within stopping distance
                animator.SetTrigger("isAttack");
                PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageOnPlayer);
                }
            }
        }
    }

    private void FacePlayer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            StartCoroutine(EnemyHit());
            health -= damage;
            if (health <= 0)
            {
                OnEnemyDeath();
            }
        }
    }

    public void OnEnemyDeath()
    {
        isDead = true;
        speed = 0f;
        animator.SetBool("isDead", true);
        boxCollider.enabled = false;

        BossSpawner.Instance.UpdateEnemyKillCounter();

        StartCoroutine(ReturnToPool());
    }

    IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(1.4f);
        spriteRenderer.color = Color.white;
        EnemyPool.Instance.ReturnEnemyToPool(gameObject);
    }

    IEnumerator EnemyHit()
    {
        animator.SetBool("isHit", true);
        speed = 0f;
        yield return new WaitForSeconds(0.01f);
        if (!isDead)
        {
            speed = originalSpeed;
            animator.SetBool("isHit", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isAttack");
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageOnPlayer);
                OnEnemyDeath();
            }
        }
    }
}
