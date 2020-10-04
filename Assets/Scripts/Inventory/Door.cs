using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Inventory inventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (inventory.HasItem("Bomb"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
