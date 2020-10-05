using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IInventoryItem
{
    //bomb as key to open the door
    private bool IsPicked = false;
    private int timer = 3000;
    private int timeCount = 0;
    public AudioClip key;
    public AudioClip pickUp;
    public string Name
    {
        get
        {
            return "Bomb";
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
        if(!IsPicked)
        {
            AudioManager.Instance.PlaySound(pickUp);
            Collider collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            IsPicked = true;
        }
        
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
            AudioManager.Instance.PlaySound(key);
            //gameObject.SetActive(false);
        }
    }

    //public void Respawn()
    //{
    //    gameObject.SetActive(true);
    //    Collider collider = gameObject.GetComponent<Collider>();
    //    collider.enabled = true;
    //}


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
                Collider collider = gameObject.GetComponent<Collider>();
                collider.enabled = true;
                gameObject.GetComponent<Renderer>().enabled = true;

                IsPicked = false;
            }
        }
    }
}
