using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        if (mItems.Count == 0)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);

                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
        else if (mItems.Count == 1)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;

                mItems.Add(mItems[0]);
                mItems[0] = item;

                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
        else
        {

            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                mItems[1].OnDrop();
                mItems[1] = mItems[0];
                //collider.enabled = false;
                mItems[0] = item;
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

    public bool HasItem(string name)
    {
        foreach(IInventoryItem item in mItems)
        {
            if(item.Name == name)
            {
                item.OnUse();
                mItems.Remove(item);
                ItemRemoved(this, new InventoryEventArgs(item));
                return true;
            }
        }
        return false;
    }
}
