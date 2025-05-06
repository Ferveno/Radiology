using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject flameParticle;
    [SerializeField] private EnemyDetectionSystem enemyDetectionSystem;

    [SerializeField] float timerForFlameThrower;
    public GameObject aim;
    private void Start()
    {
        enemyDetectionSystem = GameObject.FindAnyObjectByType<EnemyDetectionSystem>();

        if(enemyDetectionSystem.GetComponent<PlayerController>().flameThrowerParticle_Ref!=null)
        {

            flameParticle = enemyDetectionSystem.GetComponent<PlayerController>().flameThrowerParticle_Ref;

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FlameThrowerTime(timerForFlameThrower));
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            //Destroy(this.gameObject);

        }
    }

    

    void DisableAllGunsAndFirePoints()
    {
        for (int i = 0; i < enemyDetectionSystem.guns.Length; i++)
        {
            enemyDetectionSystem.guns[i].gameObject.SetActive(false);
        }

        enemyDetectionSystem.riffleFirePoint.gameObject.SetActive(false);
        enemyDetectionSystem.pistolFirePoint.gameObject.SetActive(false);

        for (int i = 0; i < enemyDetectionSystem.shotGunFirePoints.Length; i++)
        {
            enemyDetectionSystem.shotGunFirePoints[i].gameObject.SetActive(false);
        }


    }

    IEnumerator FlameThrowerTime(float timeForFlameThrower)
    {
        // GameManager.Instance.isFlameThrower = true;
        DisableAllGunsAndFirePoints();
        enemyDetectionSystem.guns[2].gameObject.SetActive(true);
        enemyDetectionSystem.riffleFirePoint.gameObject.SetActive(true);
        flameParticle.SetActive(true);

        yield return new WaitForSeconds(timeForFlameThrower);


        DisableAllGunsAndFirePoints();
        enemyDetectionSystem.guns[0].gameObject.SetActive(true);
        enemyDetectionSystem.pistolFirePoint.gameObject.SetActive(true);
        flameParticle.SetActive(false);
        // GameManager.Instance.isFlameThrower = false;

        Destroy(this.gameObject);


    }

}
