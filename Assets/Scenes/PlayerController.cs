using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Inventory inventory;
    public HUD Hud;
    private IInventoryItem mItemToPickup = null;

    private Vector3 m_movingDir = Vector3.forward;
	private float m_movingSpeed = 10f;
	[HideInInspector]
	public UnityEvent OnTurnLeft;
	[HideInInspector]
	public UnityEvent OnTurnRight;
	private CharacterController m_cc;
	private float m_jumpHeight = 2f;
	private float m_verticalSpeed;
    private void Awake()
    {
		if (OnTurnLeft is null){
			OnTurnLeft = new UnityEvent();
		}
		if (OnTurnRight is null){
			OnTurnRight = new UnityEvent();
		}
		OnTurnLeft.RemoveAllListeners();
		OnTurnRight.RemoveAllListeners();
		m_cc = GetComponent<CharacterController>();
    }

    private void TurnLeft(){
		m_movingDir = m_movingDir.TurnLeft();
		OnTurnLeft.Invoke();
    }

    private void TurnRight(){
		m_movingDir = m_movingDir.TurnRight();
		OnTurnRight.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            TurnLeft();
        }
        else if(Input.GetKeyDown(KeyCode.D)){
            TurnRight();
        }
		// Movement
        {
			var vel = m_movingDir * Time.deltaTime * m_movingSpeed;
			if (Input.GetKeyDown(KeyCode.Space) && m_cc.isGrounded){
				m_verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * m_jumpHeight);
			}
			m_verticalSpeed += Physics.gravity.y * Time.deltaTime;
			vel.y = m_verticalSpeed * Time.deltaTime;
			m_cc.Move(vel);
        }

        
        if (Input.GetKeyDown(KeyCode.F) && mItemToPickup != null)
        {
            inventory.AddItem(mItemToPickup);
            mItemToPickup.OnPickup();
            Hud.CloseMessagePanel("");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            inventory.SwitchItem();
        }

    }

    // private void FixedUpdate() {
    // 	RaycastHit hit;
    // 	if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
    // 		var pos = transform.position;
    // 		pos.y = hit.point.y + GetComponent<CapsuleCollider>().bounds.extents.y / 2;
    // 		transform.position = pos;
    // 	}
    // }


    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
    //    if(item != null)
    //    {
    //        inventory.AddItem(item);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            mItemToPickup = item;
            Hud.OpenMessagePanel("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            Hud.CloseMessagePanel("");
            mItemToPickup = null;
        }
    }
}
