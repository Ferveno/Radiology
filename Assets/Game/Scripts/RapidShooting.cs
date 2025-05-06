using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidShooting : MonoBehaviour
{
    [SerializeField] float fireRate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<EnemyDetectionSystem>().fireRate = collision.gameObject.GetComponent<EnemyDetectionSystem>().fireRate+ fireRate;

            Destroy(this.gameObject);
        }
    }
}
