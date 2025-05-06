using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public AudioSource AudioSource;
    private void OnEnable()
    {
        SoundManager.instance.Play("Pistol");
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Deactivate();
            collision.gameObject.GetComponent<Enemy>().TakeDamage(BossSpawner.Instance.player.GetComponent<PlayerController>().damageOnEnemy);
        }
        if (collision.gameObject.tag == "Ghost")
        {
            Deactivate();
            collision.gameObject.GetComponent<Enemy>().TakeDamage(BossSpawner.Instance.player.GetComponent<PlayerController>().damageOnEnemy);
        }
        else if (collision.gameObject.tag == "Boss")
        {
            Deactivate();
            collision.gameObject.GetComponent<Boss>().TakeDamage(BossSpawner.Instance.player.GetComponent<PlayerController>().damageOnEnemy);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            Deactivate();
        }
    }


    private void OnDisable()
    {
        CancelInvoke();
    }
}
