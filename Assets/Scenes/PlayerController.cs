using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 m_movingDir = Vector3.forward;
	private Camera m_camera = default;
	private CoroutineContainer m_camRotCoroutine = default;
	private float m_rotationSec = 0.5f;
    void Start()
    {
		m_camera = GetComponentInChildren<Camera>();
    }

    private void TurnLeft(){
		m_movingDir = m_movingDir.TurnLeft();
		m_camRotCoroutine.StopCoroutine();
		var targetRot = Quaternion.LookRotation(-m_camera.transform.right);
		Action afterRot = () => {
			m_camera.transform.rotation = targetRot;
		};
		m_camRotCoroutine = StartCoroutine(m_camera.gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);
    }

    private void TurnRight(){
		m_movingDir = m_movingDir.TurnRight();
		m_camRotCoroutine.StopCoroutine();
		var targetRot = Quaternion.LookRotation(m_camera.transform.right);
		Action afterRot = () => {
			m_camera.transform.rotation = targetRot;
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
        transform.position += m_movingDir * Time.deltaTime;
    }
}
