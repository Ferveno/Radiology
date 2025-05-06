using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPanel_UI : MonoBehaviour
{
    public static InventoryPanel_UI instance;

    public GameObject notEnoughCoinsPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    //    GameManager.Instance.UpdateCoins();
    }

    //public static InventoryPanel_UI ShowUI()
    //{

    //    if (instance == null)
    //    {
    //        GameObject obj = Instantiate(Resources.Load("Prefabs/InventoryPanel_UI")) as GameObject;
    //        Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
    //        for (int i = 0; i < cans.Length; i++)
    //        {
    //            if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("mainCanvas"))
    //            {
    //                obj.transform.SetParent(cans[i].transform, false);
    //                break;
    //            }
    //        }
    //        instance = obj.GetComponent<InventoryPanel_UI>();
    //    }

    //    return instance;
    //}

    public void OnBackPressed()
    {
        //Destroy(gameObject);
        UIManager.Instance.inventoryPanel.SetActive(false);
    }

    public IEnumerator DisplayNotEnoughPanel()
    {
        notEnoughCoinsPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughCoinsPanel.SetActive(false);

    }
}
