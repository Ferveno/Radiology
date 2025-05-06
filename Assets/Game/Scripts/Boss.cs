using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float originalSpeed; // Speed of the enemy
    private float speed;

    private Transform playerTransform;

    public int originalHealth;
    private int health;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int damageOnPlayer;

    bool isDead = false;

    private BoxCollider2D boxCollider;

    [SerializeField] private Slider bossHealthSlider;

    public float minCallInterval = 1f;
    public float maxCallInterval = 10f;

    // Minimum and maximum interval in seconds for the coroutine delay
    public float minCoroutineDelay = 1f;
    public float maxCoroutineDelay = 5f;

    public GameObject sphereCollider_Ref;

    public BossType bossType;

    public float stoppingDistance = 1.5f;
    public Vector2 stoppingDistanceOffset;

    [SerializeField] private bool isSpider = false; // Add a flag to check if the enemy is a spider


    public enum BossType
    {
        None,
        BigBoss,
        SmallBoss
    }

    private void Awake()
    {
        // Cache references to components in Awake to avoid multiple GetComponent calls
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        bossHealthSlider.maxValue = originalHealth;
        bossHealthSlider.value = bossHealthSlider.maxValue;
    }

    private void OnEnable()
    {
        // Reset enemy state
        bossHealthSlider.maxValue = originalHealth;
        bossHealthSlider.value = bossHealthSlider.maxValue;
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

        StartCoroutine(CallCoroutineAtRandomIntervals());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null && !isDead)
        {
            // Calculate the direction to the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position + (Vector3)stoppingDistanceOffset, playerTransform.position);

            if (distanceToPlayer > stoppingDistance)
            {
                //// Move the enemy towards the player
                //transform.position += (Vector3)direction * speed * Time.deltaTime;

                //// Make the enemy face the player
                //FacePlayer(direction);


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

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            StartCoroutine(EnemyHit());
            health -= damage;

            bossHealthSlider.value = health;
            if (health <= 0)
            {
                OnEnemyDeath();
            }
        }
    }

    void OnEnemyDeath()
    {
        isDead = true;
        speed = 0f;
        animator.SetBool("isDead", true);
        boxCollider.enabled = false;
        BossSpawner.Instance.bossKillCount++;
        //Destroy(gameObject, 1.4f);

        if (BossSpawner.Instance.bossKillCount == 3)
        {
            StartCoroutine(WaitAfterBossDeath());
        }
        else
        {
            Destroy(gameObject, 1.4f);
        }
    }

    IEnumerator WaitAfterBossDeath()
    {
        SoundManager.instance.Play("rewarded");
        GameWinPanel_UI.ShowUI();
        yield return new WaitForSeconds(1.4f);
        this.gameObject.SetActive(false);
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
            animator.SetTrigger("Attack");
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageOnPlayer);
            }
        }
    }

    IEnumerator CallCoroutineAtRandomIntervals()
    {
        while (true)
        {
            float randomCallInterval = Random.Range(minCallInterval, maxCallInterval);
            yield return new WaitForSeconds(randomCallInterval);
            StartCoroutine(ExecuteAfterRandomInterval());
        }
    }

    IEnumerator ExecuteAfterRandomInterval()
    {
        float randomDelay = Random.Range(minCoroutineDelay, maxCoroutineDelay);

        yield return new WaitForSeconds(randomDelay);
        speed = speed + 2;
        animator.SetBool("isRoll", true);

        if (bossType == BossType.BigBoss)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            sphereCollider_Ref.SetActive(true);
        }

        yield return new WaitForSeconds(4f);
        animator.SetBool("isRoll", false);

        if (bossType == BossType.BigBoss)
        {
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            sphereCollider_Ref.SetActive(false);
        }
        speed = originalSpeed;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 gizmoPosition = (Vector2)transform.position + stoppingDistanceOffset;
        Gizmos.DrawWireSphere(gizmoPosition, stoppingDistance);
    }
}
