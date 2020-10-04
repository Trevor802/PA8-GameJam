using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    string Name { get; }

    Sprite Image { get; }

    bool IsUsed { get; }

    void OnPickup();

    void OnDrop();

    void OnUse();
}

public class InventoryEventArgs : EventArgs
{
    public IInventoryItem Item;
    public InventoryEventArgs(IInventoryItem item)
    {
        Item = item;
    }
}
