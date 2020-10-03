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
        Inventory.ItemSwitched += InventoryScript_ItemSwitched;
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
        Image image = inventoryPanel.GetChild(0).GetChild(0).GetComponent<Image>();

        Image image2 = inventoryPanel.GetChild(1).GetChild(0).GetComponent<Image>();

        image2.sprite = image.sprite;
        image.sprite = e.Item.Image;
        
            //todo: store a reference to the item
    }

    private void InventoryScript_ItemSwitched(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        //find image
        Image image1 = inventoryPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        Image image2 = inventoryPanel.GetChild(1).GetChild(0).GetComponent<Image>();
        Sprite tmp = image1.sprite;
        image1.sprite = image2.sprite;
        image2.sprite = tmp;
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
