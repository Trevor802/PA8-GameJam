using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destination : MonoBehaviour
{
    public Text winText;
    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        if (winText == null)
            throw new System.Exception("Winning Text is not set");
    }


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
        winText.enabled = true;
    }
}
