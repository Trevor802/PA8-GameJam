using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.transform.right == transform.right)
                PlayerWin();

        }
    }

    private void PlayerWin()
    {
        Debug.Log("WINN");
        // TODO: Link the other levels here
    }
}
