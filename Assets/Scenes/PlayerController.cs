using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 m_movingDir = Vector3.forward;
	private CameraController m_camera = default;
	private float m_movingSpeed = 10f;
	public UnityEvent OnTurnLeft;
	public UnityEvent OnTurnRight;
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
        transform.position += m_movingDir * Time.deltaTime * m_movingSpeed;
    }
}
