using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private InterruptibleCoroutine m_camRotCoroutine = default;
	private float m_rotationSec = 0.2f;
	private bool m_rotating = false;
    void Awake()
    {
        var player = GetComponentInParent<PlayerController>();
        player.OnTurnLeft += OnTurnLeft;
        player.OnTurnRight += OnTurnRight;
		player.OnResetDirection += OnResetCamera;
    }

	private void OnResetCamera(Vector3 forward){
		transform.rotation = Quaternion.LookRotation(forward, transform.up);
	}

    private void OnTurnLeft(Vector3 forward){
		m_camRotCoroutine.StopCoroutine(this);
		var targetRot = Quaternion.LookRotation(forward, transform.up); 
		Action afterRot = () => {
			m_rotating = false;
		};
		m_rotating = true;
		m_camRotCoroutine = StartCoroutine(gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);

    }

	private void OnTurnRight(Vector3 forward){
		var targetRot = Quaternion.LookRotation(forward, transform.up);
		Action afterRot = () => {
			m_rotating = false;
		};
		m_rotating = true;
		m_camRotCoroutine = StartCoroutine(gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);
	}
}
