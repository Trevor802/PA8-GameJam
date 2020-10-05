using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Inventory inventory;
    bool CollideWithPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "Player")
        {
            CollideWithPlayer = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            CollideWithPlayer = false;
        }
    }


    void Update()
    {
        if (CollideWithPlayer)
        {
            if (inventory.HasItem("Bomb"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
