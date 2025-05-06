using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private GameObject buyButton_Ref;
    [SerializeField] private GameObject selectButton_Ref;
    [SerializeField] private int itemAmount;
    [SerializeField] private TextMeshProUGUI displayAmount_Text;
    [SerializeField] private string inventoryID;
    [SerializeField] private Button thisButton; // Reference to the button component
    [SerializeField] private string defaultGunID; // ID of the default gun

    private void Start()
    {
        displayAmount_Text.text = itemAmount.ToString();

        // Check if the item is the default gun
        if (inventoryID == defaultGunID)
        {
            buyButton_Ref.SetActive(false);
            selectButton_Ref.SetActive(true);
            PlayerPrefs.SetString("SelectedGun", defaultGunID);
            selectButton_Ref.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
            selectButton_Ref.GetComponent<Button>().interactable = false;
            return;
        }

        // Check if the item is already bought
        if (PlayerPrefs.GetInt(inventoryID + "_Bought", 0) == 1)
        {
            buyButton_Ref.SetActive(false);
            selectButton_Ref.SetActive(true);
        }

        // Check if the item is already selected
        if (PlayerPrefs.GetString("SelectedGun") == inventoryID)
        {
            selectButton_Ref.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
            selectButton_Ref.GetComponent<Button>().interactable = false;
        }
    }

    public void OnClick_BuyButton()
    {
        // if (itemAmount <= GameManager.Instance.coinAmount)
        // {
        //     GameManager.Instance.UpdateCoins(-itemAmount);
        //     PlayerPrefs.SetInt(inventoryID + "_Bought", 1);
        //     buyButton_Ref.SetActive(false);
        //     selectButton_Ref.SetActive(true);
        // }
        // else
        // {
        //     StartCoroutine(InventoryPanel_UI.instance.DisplayNotEnoughPanel());
        // }
    }

    public void OnClick_SelectButton()
    {
        // Find all InventoryItem scripts in the scene
        InventoryItem[] inventoryItems = FindObjectsOfType<InventoryItem>();

        foreach (var item in inventoryItems)
        {
            // Disable all buttons and set text to unselected
            if (item != this)
            {
                item.selectButton_Ref.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                item.selectButton_Ref.GetComponent<Button>().interactable = true;
            }
        }

        // Set the current button to selected and disable it
        PlayerPrefs.SetString("SelectedGun", inventoryID);
        selectButton_Ref.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        selectButton_Ref.GetComponent<Button>().interactable = false;
    }
}
