using UnityEngine;

public class RandomEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 movementDirection;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public int scoreToEat=1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        SetRandomDirection();
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDirection * moveSpeed;
    }

    private void SetRandomDirection()
    {
        movementDirection = Random.insideUnitCircle.normalized;
        FlipSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BounceAway(collision);

        if (collision.gameObject.tag == "Player") {
            GameManager.instance.Score = GameManager.instance.Score - scoreToEat;
            GameManager.instance.ScoreUpdater();
        }
    }

    private void BounceAway(Collision2D collision)
    {
        movementDirection = Vector2.Reflect(movementDirection, collision.contacts[0].normal);

        float randomDeviation = Random.Range(-15f, 15f);
        movementDirection = Quaternion.Euler(0, 0, randomDeviation) * movementDirection;

        movementDirection.Normalize();
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (movementDirection.x > 0)
        {
            spriteRenderer.flipX = true; // Facing right
        }
        else if (movementDirection.x < 0)
        {
            spriteRenderer.flipX = false; // Facing left
        }
    }
}
