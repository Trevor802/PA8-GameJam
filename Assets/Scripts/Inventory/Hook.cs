using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, IInventoryItem
{
    //hook to use portal
    private bool IsPicked = false;
    private int timer = 3000;
    private int timeCount = 0;
    private Vector3 originPosition;
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
        gameObject.transform.position = new Vector3(1000.0f, 1350.0f, -1020.0f);
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
        IsPicked = true;
    }

    public void OnDrop()
    {
        //Transform Player = GameObject.Find("Player").transform;

        //gameObject.SetActive(true);
        //gameObject.transform.position = Player.position;
        //Collider collider = gameObject.GetComponent<Collider>();
        //collider.enabled = true;
    }

    public void OnUse()
    {
        if (!IsUsed)
        {
            _IsUsed = true;
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        originPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if (IsPicked)
        {
            if (timeCount < timer)
            {
                timeCount++;
            }
            else
            {
                timeCount = 0;
                gameObject.transform.position = originPosition;
                Collider collider = gameObject.GetComponent<Collider>();
                collider.enabled = true;
                IsPicked = false;
            }
        }
    }
}
