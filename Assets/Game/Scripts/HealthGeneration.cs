using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGeneration : MonoBehaviour
{
    [SerializeField] float healthGenerationSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().IncreaseSpeedHealthGenerationSpeed(healthGenerationSpeed);
            Destroy(this.gameObject);
        }
    }
 
}
