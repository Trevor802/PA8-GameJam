using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    private bool IsTriggered;
    private float m_Time;//time

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.name == "Player")
        {
            if (inventory.HasItem("Knife"))
            {
                IsTriggered = true;
            }
        }
    }


    private void Update()
    {
        if (IsTriggered == true)
        {
            m_Time = Time.deltaTime + m_Time;
            transform.GetChild(0).position = Vector3.Lerp(transform.GetChild(0).position, transform.GetChild(1).position, m_Time * 0.05f);
        }
    }
}
