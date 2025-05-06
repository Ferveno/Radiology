using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 7)
        {
            Debug.Log("Hit HealthGeneration");
            collision.gameObject.GetComponent<Bullet>().Deactivate();
        }

        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().OnEnemyDeath();
        }
    }
}
