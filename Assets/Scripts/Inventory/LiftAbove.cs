using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftAbove : MonoBehaviour
{
    public bool CollideWithPlayer;

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
}
