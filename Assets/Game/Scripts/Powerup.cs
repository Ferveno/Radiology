using UnityEngine;

public class Powerup : MonoBehaviour
{
    private PowerupSpawner spawner;
    private float relocationTime;
    private float timer;

    public void Initialize(PowerupSpawner spawner, float relocationTime)
    {
        this.spawner = spawner;
        this.relocationTime = relocationTime;
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Relocate();
        }
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // Add your powerup collection logic here
    //        Destroy(gameObject);
    //    }
    //}

    void Relocate()
    {
        spawner.RelocatePowerup(gameObject);
        ResetTimer();
    }

    void ResetTimer()
    {
        timer = relocationTime;
    }
}
