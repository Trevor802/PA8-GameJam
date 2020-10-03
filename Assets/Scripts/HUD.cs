using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory Inventory;

    public GameObject MessagePanel;
    // Start is called before the first frame update
    void Start()
    {
        Inventory.ItemAdded+=InventoryScript_ItemAdded;
        Inventory.ItemUpdated += InventoryScript_ItemUpdated;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");
        foreach(Transform slot in inventoryPanel)
        {
            //find image
            Image image = slot.GetChild(0).GetComponent<Image>();
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;
                //todo: store a reference to the item
                
                break;
            }
        }
    }

    private void InventoryScript_ItemUpdated(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        //find image
        Image image = inventoryPanel.GetChild(1).GetChild(0).GetComponent<Image>();

        image.sprite = e.Item.Image;
            //todo: store a reference to the item

      
    }

    public void OpenMessagePanel(string text)
    {
        MessagePanel.SetActive(true);
    }

    public void CloseMessagePanel(string text)
    {
        MessagePanel.SetActive(false);
    }
}
