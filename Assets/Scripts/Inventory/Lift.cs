﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    private float m_Time;//time
    private bool CollideWithPlayer = false;
    private bool IsTriggered;

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.name == "Player" )
        {
            CollideWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            CollideWithPlayer = false;
        }
    }

    private void Update()
    {
        if (CollideWithPlayer && Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.HasItem("Knife"))
            {
                IsTriggered = true;
            }
        }
        if (IsTriggered)
        {
            m_Time = Time.deltaTime + m_Time;
            transform.GetChild(0).position = Vector3.Lerp(transform.GetChild(0).position, transform.GetChild(1).position, m_Time * 0.05f);
        }
    }
}
