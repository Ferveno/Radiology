using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    [SerializeField] float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>()._moveSpeed = collision.gameObject.GetComponent<PlayerController>()._moveSpeed+speed;

            Destroy(this.gameObject);
        }
    }
}
