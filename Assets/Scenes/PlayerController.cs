using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 m_movingDir = Vector3.forward;
	private Camera m_camera = default;
	private InterruptibleCoroutine m_camRotCoroutine = default;
	private float m_rotationSec = 0.5f;
	private float m_movingSpeed = 10f;
    void Start()
    {
		m_camera = GetComponentInChildren<Camera>();
    }

    private void TurnLeft(){
		m_movingDir = m_movingDir.TurnLeft();
		var prevTargetRot = m_camRotCoroutine.StopCoroutine(this, false);
		var targetRot = prevTargetRot is null ? Quaternion.LookRotation(-m_camera.transform.right) :
			(Quaternion)prevTargetRot * Quaternion.Euler(0, -90f, 0);
		InterruptibleCoroutine.Callback afterRot = (bool execute) => {
			if (execute){
				m_camera.transform.rotation = targetRot;
			}
			return targetRot;
		};
		m_camRotCoroutine = StartCoroutine(m_camera.gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);
    }

    private void TurnRight(){
		m_movingDir = m_movingDir.TurnRight();
		var prevTargetRot = m_camRotCoroutine.StopCoroutine(this, false);
		var targetRot = prevTargetRot is null ? Quaternion.LookRotation(m_camera.transform.right) :
			(Quaternion)prevTargetRot * Quaternion.Euler(0, 90f, 0);
		InterruptibleCoroutine.Callback afterRot = (bool execute) => {
			if (execute){
				m_camera.transform.rotation = targetRot;
			}
			return targetRot;
		};
		m_camRotCoroutine = StartCoroutine(m_camera.gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);
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
