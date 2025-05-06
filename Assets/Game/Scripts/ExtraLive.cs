using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLive : MonoBehaviour
{
    [SerializeField] private int liveToAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().UpdatePlayerLive(liveToAdd);
            Destroy(this.gameObject);
        }
    }
}
