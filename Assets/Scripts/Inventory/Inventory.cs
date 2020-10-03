using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 2;
    private List<IInventoryItem> mItems = new List<IInventoryItem>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemUpdated;
    public event EventHandler<InventoryEventArgs> ItemSwitched;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    public void AddItem(IInventoryItem item)
    {
        //<slot
        if(mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);
                item.OnPickup();

                if(ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
        else
        {
            mItems[1].OnDrop();
            mItems.RemoveAt(1);
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);
                item.OnPickup();

                if (ItemUpdated != null)
                {
                    ItemUpdated(this, new InventoryEventArgs(item));
                }
            }
        }
    }

    public void SwitchItem()
    {
        if(mItems.Count == SLOTS)
        {
            IInventoryItem item2 = mItems[0];
            IInventoryItem item1 = mItems[1];
            mItems.Clear();
            mItems.Add(item1);
            mItems.Add(item2);
            if (ItemSwitched != null)
            {
                ItemSwitched(this, new InventoryEventArgs(item1));
            }
        }
        
    }
    
}
