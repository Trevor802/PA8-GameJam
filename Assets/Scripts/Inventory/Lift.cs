using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    private float m_Time;//time
    private bool IsTriggered;
    public LiftAbove LiftAbove;
    public LiftBelow LiftBelow;
    public Vector3 AboveOriginTransform;
    public Vector3 BelowOriginTransform;

    private bool EndLift = true;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name == "Player")
    //    {
    //        EndLift = false;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name == "Player")
    //    {
    //        EndLift = true;
    //    }
    //}

    private void Awake()
    {
        AboveOriginTransform = transform.GetChild(0).position;
        BelowOriginTransform = transform.GetChild(1).position;
    }
    private void Update()
    {
        if (LiftBelow.CollideWithPlayer && Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.HasItem("Knife"))
            {
                IsTriggered = true;
                m_Time = 0;
            }
        }
        if (LiftBelow.CollideWithPlayer && IsTriggered)
        {
            m_Time = Time.deltaTime + m_Time;
            transform.GetChild(1).position = Vector3.Lerp(transform.GetChild(1).position, transform.GetChild(0).position, m_Time * 0.05f);
        }
        

        if ( LiftAbove.CollideWithPlayer && Input.GetKeyDown(KeyCode.F))
        {
            IsTriggered = true;
            m_Time = 0;
        }
        if (LiftAbove.CollideWithPlayer && IsTriggered)
        {
            m_Time = Time.deltaTime + m_Time;
            transform.GetChild(0).position = Vector3.Lerp(transform.GetChild(0).position, transform.GetChild(1).position, m_Time * 0.05f);
        }

        if (!LiftAbove.CollideWithPlayer&&!LiftBelow.CollideWithPlayer&& Input.GetKeyDown(KeyCode.F))
        {
            IsTriggered = false;
            transform.GetChild(0).position = AboveOriginTransform;
            transform.GetChild(1).position = BelowOriginTransform;
        }

    }
}
