using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 m_movingDir = Vector3.forward;
	private float m_movingSpeed = 10f;
	[HideInInspector]
	public UnityEvent OnTurnLeft;
	[HideInInspector]
	public UnityEvent OnTurnRight;
	private CharacterController m_cc;
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
        var vel = m_movingDir * Time.deltaTime * m_movingSpeed;
		if (Input.GetKeyDown(KeyCode.Space) && m_cc.isGrounded){
			vel.y += Mathf.Sqrt(Physics.gravity.y * -3f);
		}
		vel += Physics.gravity * Time.deltaTime;
		m_cc.Move(vel);
    }

	// private void FixedUpdate() {
	// 	RaycastHit hit;
	// 	if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
	// 		var pos = transform.position;
	// 		pos.y = hit.point.y + GetComponent<CapsuleCollider>().bounds.extents.y / 2;
	// 		transform.position = pos;
	// 	}
	// }
}
