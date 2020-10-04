using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "Hook";
        }
    }

    public Sprite _Image = null;
    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public bool _IsUsed = false;
    public bool IsUsed
    {
        get
        {
            return _IsUsed;
        }
    }

    public void OnPickup()
    {
        //TODO : add logic what happens when hook is picked up by player
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        Transform Player = GameObject.Find("Player").transform;

        gameObject.SetActive(true);
        gameObject.transform.position = Player.position;
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;
    }

    public void OnUse()
    {
        if (!IsUsed)
        {
            _IsUsed = true;
            gameObject.SetActive(false);
        }
    }
}
